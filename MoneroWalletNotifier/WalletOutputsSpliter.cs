using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using xmr_tutorials.Rpc;
using xmr_tutorials.Utils;
using xmr_tutorials.Wallet;
using xmr_tutorials.Wallet.Payload;

namespace xmr_tutorials.Wallet
{
    public class WalletOutputsSpliter : AsyncSafeTimer
    {
        private readonly WalletManager wallet;

        private ulong splitAmount =>
            MoneroUtils.MoneroToAtomic(config.SplitAmount);

        private uint outputsPerTransfer => config.MaxOutputsPerTransfer;

        private readonly ILogger logger;
        private WalletBackgroundConfig config;

        protected override TimeSpan UpdateInterval =>
            TimeSpan.FromMinutes(config.UpdateEveryMinutes);

        public WalletOutputsSpliter(
            WalletManager wallet,
            ILoggerFactory loggerFactory,
            IOptions<WalletConfiguration> options)
        {
            this.wallet = wallet;
            this.logger = loggerFactory.CreateLogger<WalletOutputsSpliter>();
            this.config = options.Value.BackgroundConfig;
        }

        public override void StartTimer()
        {
            if (config.AttemptToSplitOutputs)
            {
                logger.LogInformation($"Will attempt to split wallet outputs every {config.UpdateEveryMinutes} minutes");
                base.StartTimer();
            }
            else
            {
                logger.LogInformation($"Wallet outputs will NOT be auto-split in background");
            }
        }

        protected override Task Tick()
        {
            return TrySplitOutputs();
        }

        private async Task TrySplitOutputs()
        {
            var transfers = await this.wallet
                .QueryIncomingTransfersAsync(IncomingTransferType.Available);
            var transfersToSplit = transfers
                .Where(t => t.Amount > splitAmount)
                .OrderBy(t => t.Amount);

            logger.LogInformation($"{transfersToSplit.Count()} can be split");

            if (transfersToSplit.Any())
            {
                var address = await wallet.QueryAddressAsync();
                foreach (var transfer in transfersToSplit)
                {
                    var outputAmount = transfer.Amount / splitAmount;
                    if (outputAmount > outputsPerTransfer)
                    {
                        outputAmount = outputsPerTransfer;
                    }

                    logger.LogInformation($"Split {MoneroUtils.AtomicToMonero(transfer.Amount)}" +
                        $" into {outputAmount} of {MoneroUtils.AtomicToMonero(splitAmount)}");
                    var transaction = new TransferPayloadDto();
                    for (uint i = 0; i < outputsPerTransfer; i++)
                    {
                        transaction.Destinations.Add(new TransferPayloadDestinationDto
                        {
                            Address = address,
                            Amount = splitAmount
                        });
                    }

                    logger.LogInformation($"Executing transfer..");
                    try
                    {
                        var transferResult = await wallet.TransferAsync(transaction);
                        logger.LogInformation($"Transfer {MoneroUtils.AtomicToMonero(transferResult.Amount)}" +
                            $" with fee {MoneroUtils.AtomicToMonero(transferResult.Fee)} ({ transferResult.TxHash})");
                    }
                    catch (RpcResponseException ex)
                    {
                        if (ex.ResponseError.Code == -37 || ex.ResponseError.Code == -17)
                        {
                            // not enough money. Fee is not considered.
                            logger.LogWarning($"Transfer of {MoneroUtils.AtomicToMonero(splitAmount)} failed due to {ex.Message}. Stopping.");
                            return;
                        }
                        throw;
                    }
                }

            }
        }
    }
}