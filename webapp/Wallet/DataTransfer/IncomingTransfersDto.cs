using System.Collections.Generic;
using Newtonsoft.Json;

namespace xmr_tutorials.Wallet.DataTransfer
{
    public class IncomingTransferResultDto
    {
        public IncomingTransferResultDto()
        {
            Transfers = new List<IncomingTransferDto>();
        }

        [JsonProperty("transfers")]
        public IEnumerable<IncomingTransferDto> Transfers { get; set; }
    }

    public class IncomingTransferDto
    {
        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("global_index")]
        public uint GlobalIndex { get; set; }

        [JsonProperty("key_image")]
        public string KeyImage { get; set; }

        [JsonProperty("spent")]
        public bool Spent { get; set; }

        [JsonProperty("subaddr_index")]
        public IncomingTransferSubaddrIndex SubaddrIndex { get; set; }

        [JsonProperty("tx_hash")]
        public string TxHash { get; set; }
    }

    public class IncomingTransferSubaddrIndex
    {
        [JsonProperty("major")]
        public uint Major { get; set; }

        [JsonProperty("minor")]
        public uint Minor { get; set; }
    }
}