using System;
using System.Threading;
using System.Threading.Tasks;

namespace xmr_tutorials.Utils
{
    public abstract class AsyncSafeTimer
    {
        protected virtual TimeSpan UpdateInterval => TimeSpan.FromMilliseconds(250);
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private Timer timer;
        private volatile bool updating;

        public virtual void StartTimer()
        {
            if (timer != null)
            {
                throw new InvalidOperationException("This operation is already scheduled");
            }

            timer = new Timer(OnTimer, null, TimeSpan.FromSeconds(0), UpdateInterval);
        }

        protected abstract Task Tick();

        private async void OnTimer(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            await semaphore.WaitAsync();
            try
            {
                if (!updating)
                {
                    updating = true;

                    await Tick();

                    updating = false;
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

    }
}