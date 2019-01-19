using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneroClient.Rpc;
using MoneroClient.Utils;
using MoneroClient.Wallet;
using MoneroClient.Wallet.DataTransfer;
using MoneroClient.Wallet.Payload;

namespace MoneroWalletNotifier
{
    public class WalletOutputsDicer
    {
        private readonly WalletManager wallet;

        private ulong SplitAmount =>
            MoneroUtils.MoneroToAtomic(config.SplitAmount);

        private uint OutputsPerTransfer => config.MaxOutputsPerTransfer;

        private readonly ILogger logger;

        private DicingConfiguration config;

        public WalletOutputsDicer(
            WalletManager wallet,
            ILoggerFactory loggerFactory,
            IOptions<DicingConfiguration> dicingOptions)
        {
            this.wallet = wallet;
            logger = loggerFactory.CreateLogger<WalletOutputsDicer>();
            config = dicingOptions.Value;
        }

        public async Task TryDice()
        {
            if (!config.Enabled)
            {
                logger.LogInformation("Dicing disabled by config");
                return;
            }

            var transfers = await wallet
                .QueryIncomingTransfersAsync(IncomingTransferType.Available);

            var address = await wallet.QueryAddressAsync();

            var newTransfers = PerformDice(transfers, address);
            foreach (var transfer in newTransfers)
            {
                await PerformTransfer(transfer);
            }
        }

        protected IEnumerable<TransferPayloadDto> PerformDice(
            IEnumerable<IncomingTransferDto> transfers,
            string targetAddress)
        {
            var transfersToSplit = transfers
                .Where(t => t.Amount > SplitAmount)
                .OrderBy(t => t.Amount);

            logger.LogInformation($"{transfersToSplit.Count()} will be diced.");

            var newTransfers = new List<TransferPayloadDto>();
            TransferPayloadDto currentTransfer = null;

            foreach (var transfer in transfersToSplit)
            {
                var transferAmount = transfer.Amount / SplitAmount;

                logger.LogInformation($"Dicing {MoneroUtils.AtomicToMonero(transfer.Amount)}" +
                    $" into {transferAmount} of {MoneroUtils.AtomicToMonero(SplitAmount)}");

                for (uint i = 0; i < transferAmount; i++)
                {
                    if (currentTransfer == null
                        || currentTransfer.Destinations.Count >= OutputsPerTransfer)
                    {
                        currentTransfer = new TransferPayloadDto();
                        newTransfers.Add(currentTransfer);
                    }

                    currentTransfer.Destinations.Add(new TransferPayloadDestinationDto
                    {
                        Address = targetAddress,
                        Amount = SplitAmount
                    });
                }
            }
            return newTransfers;
        }

        protected async Task PerformTransfer(TransferPayloadDto transaction)
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
                        logger.LogWarning($"Transfer of {MoneroUtils.AtomicToMonero(SplitAmount)} failed due to {ex.Message}. Stopping.");
                        return;
                    }
                    throw;
                }
            }
        }
    }
}