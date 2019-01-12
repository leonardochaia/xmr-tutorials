using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MoneroClient.Wallet;

namespace xmr_tutorials.AppStatus
{
    public class AppStatusHub : Hub
    {
        private readonly WalletManager wallet;
        private readonly AppStatusHubBroadcaster broadcaster;

        public AppStatusHub(
            WalletManager wallet,
            AppStatusHubBroadcaster broadcaster)
        {
            this.wallet = wallet;
            this.broadcaster = broadcaster;
        } 

        public override Task OnConnectedAsync()
        {
            return broadcaster.BroadcastCurrentStatus(Clients.Caller);
        }
    }
}