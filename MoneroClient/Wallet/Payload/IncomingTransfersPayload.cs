using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MoneroClient.Wallet.Payload
{
    public enum IncomingTransferType
    {
        [EnumMember(Value = "all")]
        All = 1,

        [EnumMember(Value = "available")]
        Available = 2,

        [EnumMember(Value = "unavailable")]
        Unavailable = 2
    }

    public class IncomingTransfersPayload
    {
        [JsonProperty("transfer_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public IncomingTransferType TransferType { get; set; }
    }
}