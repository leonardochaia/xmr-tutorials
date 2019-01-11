using Newtonsoft.Json;

namespace MoneroClient.Rpc
{
    public class RpcRequestPayload
    {
        [JsonProperty("jsonrpc")]
        private static string Version { get { return "2.0"; } }

        [JsonProperty("method")]
        public string Method { get; }

        public RpcRequestPayload(string method)
        {
            Method = method;
        }
    }

    public class RpcRequestPayload<T> : RpcRequestPayload
    {
        [JsonProperty("params")]
        public T Parameters { get; }

        public RpcRequestPayload(string method, T parameters) : base(method)
        {
            Parameters = parameters;
        }
    }
}