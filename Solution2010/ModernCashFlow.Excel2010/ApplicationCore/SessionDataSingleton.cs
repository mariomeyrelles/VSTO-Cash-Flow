using System.Collections.Generic;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Simple structure to store lists of basic domain entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SessionDataSingleton<T>
    {
        private static List<T> _instance;

        public static List<T> Instance
        {
            get { return _instance ?? (_instance = new List<T>()); }
        }
    }
}
