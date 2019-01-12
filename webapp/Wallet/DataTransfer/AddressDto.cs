using Newtonsoft.Json;

namespace xmr_tutorials.Wallet.DataTransfer
{
    public class AddressDto
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("addresses")]
        public Address[] Addresses { get; set; }
    }

    public class Address
    {
        [JsonProperty("address")]
        public string AddressAddress { get; set; }

        [JsonProperty("address_index")]
        public uint AddressIndex { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }
    }
}