using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using xmr_tutorials.Rpc;
using xmr_tutorials.Utils;
using xmr_tutorials.Wallet.DataTransfer;
using xmr_tutorials.Wallet.Payload;

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
            return MoneroUtils.AtomicToMonero(response.Result.Balance);
        }

        public async Task<TransferResultDto> TransferAsync(TransferPayloadDto dto)
        {
            var payload = new RpcRequestPayload<TransferPayloadDto>("transfer", dto);
            var response = await this.CallAsync<TransferResultDto>(payload);
            return response.Result;
        }

        public async Task<IEnumerable<IncomingTransferDto>> QueryIncomingTransfersAsync(IncomingTransferType transferType)
        {
            var payload = new RpcRequestPayload<IncomingTransfersPayload>("incoming_transfers", new IncomingTransfersPayload
            {
                TransferType = transferType
            });
            var response = await this.CallAsync<IncomingTransferResultDto>(payload);
            return response.Result.Transfers;
        }

        internal async Task<RpcResponse<TResult>> CallAsync<TResult>(RpcRequestPayload payload)
        {
            return await client.CallAsync<TResult>($"{config.Url}/json_rpc", payload);
        }
    }
}