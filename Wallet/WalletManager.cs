using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using xmr_tutorials.Rpc;
using xmr_tutorials.Wallet.DataTransfer;

namespace xmr_tutorials.Wallet
{
    public class WalletManager
    {
        private readonly WalletConfiguration config;

        private readonly RpcClient client;

        public WalletManager(
            IOptions<WalletConfiguration> options,
            RpcClient client)
        {
            this.config = options.Value;
            this.client = client;
        }

        public async Task<string> QueryAddressAsync()
        {
            var response = await this.CallAsync<AddressDto>(new RpcRequestPayload("get_address"));
            return response.Result.Address;
        }

        public async Task<BalanceDto> QueryBalanceAsync()
        {
            var response = await this.CallAsync<BalanceDto>(new RpcRequestPayload("get_balance"));
            return response.Result;
        }

        public async Task<double> QueryHumanFriendlyBalanceAsync()
        {
            var response = await this.CallAsync<BalanceDto>(new RpcRequestPayload("get_balance"));
            // 1e-12 XMR (0.000000000001 XMR, or one piconero)
            return response.Result.Balance / 1e12;
        }

        internal async Task<RpcResponse<TResult>> CallAsync<TResult>(RpcRequestPayload payload)
        {
            return await client.CallAsync<TResult>($"{config.Url}/json_rpc", payload);
        }
    }
}