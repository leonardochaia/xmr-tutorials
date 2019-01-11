using System;
using Newtonsoft.Json;

namespace MoneroClient.Rpc
{
    public class RpcResponseException : Exception
    {
        public RpcResponseError ResponseError { get; }
        public RpcResponseException(RpcResponseError responseError)
        : base(responseError.Message)
        {
            ResponseError = responseError ?? throw new ArgumentNullException(nameof(responseError));
        }

    }
}