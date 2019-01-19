using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneroClient.Wallet;
using MoneroClient.Wallet.DataTransfer;
using MoneroClient.Wallet.Payload;
using System.Collections.Generic;

namespace MoneroWalletNotifier.Test
{
    public class WalletDicerForTesting : WalletOutputsDicer
    {
        public WalletDicerForTesting(
            WalletManager wallet,
            ILoggerFactory loggerFactory,
            IOptions<DicingConfiguration> dicingOptions)
            : base(wallet, loggerFactory, dicingOptions)
        {
        }

        public IEnumerable<TransferPayloadDto> PerformDiceTest(
            IEnumerable<IncomingTransferDto> transfers,
            string targetAddress)
        {
            return base.PerformDice(transfers, targetAddress);
        }
    }
}
