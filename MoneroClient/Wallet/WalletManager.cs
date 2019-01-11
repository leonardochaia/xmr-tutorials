using System.Threading.Tasks;
using MoneroClient.Rpc;
using MoneroClient.Wallet.DataTransfer;
using MoneroClient.Wallet.Payload;

namespace MoneroClient.Wallet
{
    public class WalletManager
    {
        private readonly WalletConfiguration config;

        private readonly RpcClient client;

        public WalletManager(
            WalletConfiguration config,
            RpcClient client)
        {
            this.config = config;
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

        public async Task<TransferResultDto> TransferAsync(string recipient, ulong atomicAmount)
        {
            var payload = new RpcRequestPayload<TransferPayloadDto>("transfer", new TransferPayloadDto
            {
                Destinations = new[] {
                    new TransferPayloadDestinationDto(){
                        Address = recipient,
                        Amount = atomicAmount
                    }
                }
            });
            var response = await this.CallAsync<TransferResultDto>(payload);
            return response.Result;
        }

        public async Task<RpcResponse<TResult>> CallAsync<TResult>(RpcRequestPayload payload)
        {
            return await client.CallAsync<TResult>($"{config.Url}/json_rpc", payload);
        }
    }
}