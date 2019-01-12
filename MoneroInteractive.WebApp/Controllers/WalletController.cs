using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneroClient.Rpc;
using MoneroClient.Wallet;

namespace MoneroInteractive.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        private readonly WalletManager wallet;

        public WalletController(WalletManager wallet)
        {
            this.wallet = wallet;
        }

#if DEBUG
        /// <summary>
        /// Quick way to execute an RPC method on the wallet.
        /// No payloads
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<dynamic> Test(string method = "get_balance")
        {
            return await wallet.CallAsync<dynamic>(new RpcRequestPayload(method));
        }

        [HttpGet("address")]
        public async Task<string> Address()
        {
            return await wallet.QueryAddressAsync();
        }

        [HttpGet("balance")]
        public async Task<double> Balance()
        {
            return await wallet.QueryHumanFriendlyBalanceAsync();
        }
#endif
    }
}