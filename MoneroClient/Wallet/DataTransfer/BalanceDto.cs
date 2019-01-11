using Newtonsoft.Json;

namespace MoneroClient.Wallet.DataTransfer
{
    public class BalanceDto
    {
        [JsonProperty("balance")]
        public ulong Balance { get; set; }

        [JsonProperty("multisig_import_needed")]
        public bool MultisigImportNeeded { get; set; }

        [JsonProperty("per_subaddress")]
        public PerSubaddress[] PerSubaddress { get; set; }

        [JsonProperty("unlocked_balance")]
        public ulong UnlockedBalance { get; set; }
    }

    public class PerSubaddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("address_index")]
        public int AddressIndex { get; set; }

        [JsonProperty("balance")]
        public ulong Balance { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("num_unspent_outputs")]
        public int NumUnspentOutputs { get; set; }

        [JsonProperty("unlocked_balance")]
        public ulong UnlockedBalance { get; set; }
    }
}