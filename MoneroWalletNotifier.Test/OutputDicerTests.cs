using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneroClient.Utils;
using MoneroClient.Wallet;
using MoneroClient.Wallet.DataTransfer;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace MoneroWalletNotifier.Test
{
    [TestClass]
    public class OutputDicerTests
    {
        WalletDicerForTesting dicer;
        readonly DicingConfiguration config = new DicingConfiguration
        {
            SplitAmount = 100,
            MaxOutputsPerTransfer = 10,
            DryRun = true,
            Enabled = true,
        };
        readonly string testAddress = "test_address";

        [TestInitialize]
        public void TestInitialize()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(opt =>
                {
                    opt.AddConsole();
                    opt.SetMinimumLevel(LogLevel.Trace);
                })
                .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            var options = Mock.Of<IOptions<DicingConfiguration>>(op => op.Value == config);
            dicer = new WalletDicerForTesting(null, loggerFactory, options);
        }

        [TestMethod]
        public void DicerShouldDiceOutputsAccordingToConfiguration()
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
    }
}
