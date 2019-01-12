using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneroClient.Rpc;
using MoneroClient.Utils;
using MoneroClient.Wallet;
using MoneroClient.Wallet.Payload;
using Newtonsoft.Json;

namespace MoneroWalletNotifier
{
    public class WalletOutputsSpliter
    {
        private readonly WalletManager wallet;

        private ulong splitAmount =>
            MoneroUtils.MoneroToAtomic(config.SplitAmount);

        private uint outputsPerTransfer => config.MaxOutputsPerTransfer;

        private readonly ILogger logger;

        private WalletBackgroundConfig config;

        public WalletOutputsSpliter(
            WalletManager wallet,
            ILoggerFactory loggerFactory,
            WalletConfiguration walletConfig)
        {
            this.wallet = wallet;
            this.logger = loggerFactory.CreateLogger<WalletOutputsSpliter>();
            this.config = walletConfig.BackgroundConfig;
        }

        public async Task TrySplitOutputs()
        {
            if (!config.AttemptToSplitOutputs)
            {
                logger.LogInformation("Outputs splitting disabled by config");
                return;
            }

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
                    var transferAmount = transfer.Amount / splitAmount;
                    if (transferAmount > outputsPerTransfer)
                    {
                        transferAmount = outputsPerTransfer;
                    }

                    logger.LogInformation($"Split {MoneroUtils.AtomicToMonero(transfer.Amount)}" +
                        $" into {transferAmount} of {MoneroUtils.AtomicToMonero(splitAmount)}");

                    var transaction = new TransferPayloadDto();
                    for (uint i = 0; i < transferAmount; i++)
                    {
                        transaction.Destinations.Add(new TransferPayloadDestinationDto
                        {
                            Address = address,
                            Amount = splitAmount
                        });
                    }

                    await PerformTransfer(transaction);
                }
            }
        }

        private async Task PerformTransfer(TransferPayloadDto transaction)
        {
            if (config.DryRun)
            {
                var amounts = transaction.Destinations.Select(d => MoneroUtils.AtomicToMonero(d.Amount));
                logger.LogInformation("DRY RUN: Transfer " + string.Join(',', amounts));
            }
            else
            {
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