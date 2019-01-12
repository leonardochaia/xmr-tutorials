namespace MoneroClient.Wallet
{
    public class WalletConfiguration
    {
        public string Url { get; set; }

        public WalletBackgroundConfig BackgroundConfig { get; set; }
    }

    public class WalletBackgroundConfig
    {
        public bool AttemptToSplitOutputs { get; set; } = true;

        public bool DryRun { get; set; } = true;

        public uint UpdateEveryMinutes { get; set; } = 10;

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