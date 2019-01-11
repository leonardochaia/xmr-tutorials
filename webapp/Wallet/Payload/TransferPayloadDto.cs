using System.Collections.Generic;
using Newtonsoft.Json;

namespace xmr_tutorials.Wallet.Payload
{
    public class TransferPayloadDto
    {
        [JsonProperty("destinations")]
        public IEnumerable<TransferPayloadDestinationDto> Destinations { get; set; }

        [JsonProperty("account_index", NullValueHandling = NullValueHandling.Ignore)]
        public long? AccountIndex { get; set; }

        [JsonProperty("subaddr_indices", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<long> SubaddrIndices { get; set; }

        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public long? Priority { get; set; }

        [JsonProperty("ring_size", NullValueHandling = NullValueHandling.Ignore)]
        public long? RingSize { get; set; }

        [JsonProperty("get_tx_key", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GetTxKey { get; set; }
    }

    public class TransferPayloadDestinationDto
    {
        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}