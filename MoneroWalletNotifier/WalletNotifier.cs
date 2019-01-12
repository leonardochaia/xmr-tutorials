using System;
using System.Threading;
using System.Threading.Tasks;
using MoneroClient.Wallet;

namespace MoneroWalletNotifier
{
    public class WalletNotifier
    {
        private readonly WalletManager wallet;
        private Timer timer;

        public WalletNotifier(WalletManager wallet)
        {
            this.wallet = wallet;
            var updateInterval = TimeSpan.FromSeconds(10);
            timer = new Timer(OnTimer, null, TimeSpan.FromSeconds(0), updateInterval);
        }

        public async void OnTimer(object state)
        {

        }
    }
}