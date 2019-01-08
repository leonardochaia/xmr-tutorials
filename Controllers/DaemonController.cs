using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using xmr_tutorials.Configuration;
using xmr_tutorials.Rpc;

namespace xmr_tutorials.Controllers
{
    [Route("api/[controller]")]
    public class DaemonController : Controller
    {
        private readonly DaemonConfiguration config;

        private readonly RpcClient client;

        public DaemonController(
            IOptions<DaemonConfiguration> options,
            RpcClient client)
        {
            this.config = options.Value;
            this.client = client;
        }

        [HttpGet]
        public async Task<dynamic> Test(string method = "get_info")
        {
            return await client.Call<dynamic>($"{config.Url}/json_rpc", method);
        }
    }
}