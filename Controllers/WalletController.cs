using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using xmr_tutorials.Configuration;
using xmr_tutorials.Rpc;

namespace xmr_tutorials.Controllers
{
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        private readonly WalletConfiguration config;

        private readonly RpcClient client;

        public WalletController(
            IOptions<WalletConfiguration> options,
            RpcClient client)
        {
            this.config = options.Value;
            this.client = client;
        }

        [HttpGet]
        public async Task<dynamic> Test(string method = "get_balance")
        {
            return await client.Call<dynamic>($"{config.Url}/json_rpc", method);
        }
    }
}