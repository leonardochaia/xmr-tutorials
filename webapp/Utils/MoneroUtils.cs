namespace xmr_tutorials.Utils
{
    public static class MoneroUtils
    {
        public static double AtomicToMonero(ulong atomic)
        {
            // 1e-12 XMR (0.000000000001 XMR, or one piconero)
            return atomic / 1e12;
        }

        public static ulong MoneroToAtomic(double xmr)
        {
            return (ulong)(xmr * 1e12);
        }
    }
}