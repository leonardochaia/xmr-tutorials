
namespace MoneroWalletNotifier
{
    public class DicingConfiguration
    {
        public bool Enabled { get; set; } = true;

        public bool DryRun { get; set; } = true;

        /// <summary>
        /// Split incoming transfers of more than SplitAmount
        /// into transfers of SplitAmount
        /// </summary>
        public uint SplitAmount { get; set; } = 100;

        /// <summary>
        /// Max outputs to include in a single transfer
        /// </summary>
        public uint MaxOutputsPerTransfer { get; set; } = 10;
    }
}