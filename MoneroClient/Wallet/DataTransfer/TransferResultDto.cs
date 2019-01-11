using Newtonsoft.Json;

namespace MoneroClient.Wallet.DataTransfer
{
    public class TransferResultDto
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("multisig_txset")]
        public string MultisigTxset { get; set; }

        [JsonProperty("tx_blob")]
        public string TxBlob { get; set; }

        [JsonProperty("tx_hash")]
        public string TxHash { get; set; }

        [JsonProperty("tx_key")]
        public string TxKey { get; set; }

        [JsonProperty("tx_metadata")]
        public string TxMetadata { get; set; }

        [JsonProperty("unsigned_txset")]
        public string UnsignedTxset { get; set; }
    }
}