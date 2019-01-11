using Newtonsoft.Json;

namespace MoneroClient.Rpc
{
    public class RpcResponse<T>
    {
        [JsonProperty("result")]
        public T Result { get; private set; }

        [JsonProperty("error")]
        public RpcResponseError Error { get; private set; }
    }

    public class RpcResponseError
    {
        [JsonProperty("code")]
        public int Code { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}