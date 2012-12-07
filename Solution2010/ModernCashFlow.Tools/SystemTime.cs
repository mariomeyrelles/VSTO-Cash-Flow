using System;

namespace ModernCashFlow.Tools
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
        public static Func<DateTime> Today = () => DateTime.Now.Today();
    }
}