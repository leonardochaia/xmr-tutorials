using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneroClient.Utils;
using MoneroClient.Wallet.DataTransfer;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace MoneroWalletNotifier.Test
{
    [TestClass]
    public class OutputDicerTests
    {
        private WalletDicerForTesting dicer;

        private readonly DicingConfiguration config = new DicingConfiguration
        {
            SplitAmount = 100,
            MaxOutputsPerTransfer = 10,
            DryRun = true,
            Enabled = true,
        };

        private readonly string testAddress = "test_address";

        [TestInitialize]
        public void TestInitialize()
        {
            var logger = Mock.Of<ILogger<WalletOutputsDicer>>();
            var options = Mock.Of<IOptions<DicingConfiguration>>(op => op.Value == config);
            dicer = new WalletDicerForTesting(null, logger, options);
        }

        [TestMethod]
        public void ShouldDiceAccordingToConfiguration()
        {
            var transfers = new List<IncomingTransferDto>()
            {
                new IncomingTransferDto
                {
                    Amount = MoneroUtils.MoneroToAtomic(1500),
                }
            };

            var result = dicer.PerformDiceTest(transfers, testAddress);

            // 2 transfers
            Assert.AreEqual(2, result.Count());

            // first 10 of 100, second 5 of 100
            Assert.AreEqual(10, result.ElementAt(0).Destinations.Count);
            Assert.AreEqual(5, result.ElementAt(1).Destinations.Count);

            Assert.IsTrue(result.All(t => t.Destinations.All(d => d.Amount == MoneroUtils.MoneroToAtomic(config.SplitAmount))));
            Assert.IsTrue(result.All(t => t.Destinations.All(d => d.Address == testAddress)));
        }

        [TestMethod]
        public void ShouldNotDiceIfNoIncomingTransfers()
        {
            var transfers = new List<IncomingTransferDto>();

            var result = dicer.PerformDiceTest(transfers, testAddress);

            Assert.IsFalse(result.Any());
        }
    }
}
