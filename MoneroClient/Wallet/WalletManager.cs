using System.Collections.Generic;
using System.Threading.Tasks;
using MoneroClient.Rpc;
using MoneroClient.Utils;
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
            var response = await CallAsync<AddressDto>(new RpcRequestPayload("get_address"));
            return response.Result.Address;
        }

        public async Task<BalanceDto> QueryBalanceAsync()
        {
            var response = await CallAsync<BalanceDto>(new RpcRequestPayload("get_balance"));
            return response.Result;
        }

        public async Task<double> QueryHumanFriendlyBalanceAsync()
        {
            var response = await CallAsync<BalanceDto>(new RpcRequestPayload("get_balance"));
            return MoneroUtils.AtomicToMonero(response.Result.Balance);
        }

        public async Task<TransferResultDto> TransferAsync(TransferPayloadDto dto)
        {
            var payload = new RpcRequestPayload<TransferPayloadDto>("transfer", dto);
            var response = await CallAsync<TransferResultDto>(payload);
            return response.Result;
        }

        public async Task<IEnumerable<IncomingTransferDto>> QueryIncomingTransfersAsync(IncomingTransferType transferType)
        {
            var payload = new RpcRequestPayload<IncomingTransfersPayload>("incoming_transfers", new IncomingTransfersPayload
            {
                TransferType = transferType
            });
            var response = await CallAsync<IncomingTransferResultDto>(payload);
            return response.Result.Transfers;
        }

        public async Task<RpcResponse<TResult>> CallAsync<TResult>(RpcRequestPayload payload)
        {
            return await client.CallAsync<TResult>($"{config.Url}/json_rpc", payload);
        }
    }
}