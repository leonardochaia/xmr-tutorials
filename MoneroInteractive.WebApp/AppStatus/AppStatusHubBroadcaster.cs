using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MoneroClient.Wallet;
using MoneroInteractive.WebApp.AppStatus.DataTransfer;
using MoneroInteractive.WebApp.Utils;

namespace MoneroInteractive.WebApp.AppStatus
{
    public class AppStatusHubBroadcaster : AsyncSafeTimer
    {
        protected override TimeSpan UpdateInterval => TimeSpan.FromSeconds(10);
        private readonly WalletManager wallet;
        private readonly IHubContext<AppStatusHub> hubContext;

        private volatile AppStatusDto currentStatus;

        public AppStatusHubBroadcaster(
            WalletManager wallet,
            IHubContext<AppStatusHub> hubContext)
        {
            this.wallet = wallet;
            this.hubContext = hubContext;
        }

        public Task BroadcastCurrentStatus(IClientProxy proxy)
        {
            return proxy.SendAsync("statusChanged", currentStatus);
        }

        protected override async Task Tick()
        {
            var balance = await this.wallet.QueryBalanceAsync();
            var newStatus = new AppStatusDto
            {
                HasEnoughBalance = balance.UnlockedBalance > 0
            };

            if (!newStatus.Equals(currentStatus))
            {
                currentStatus = newStatus;
                await BroadcastCurrentStatus(hubContext.Clients.All);
            }
        }
    }
}