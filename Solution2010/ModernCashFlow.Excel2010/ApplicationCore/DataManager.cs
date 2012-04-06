using System.Collections.Generic;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    public class SessionDataSingleton<T>
    {
        private static List<T> _instance;

        //todo: verificar se preciso recuperar os dados de algum lugar quando a lista já e estiver vazia. Cabe evento?
        public static List<T> Instance
        {
            get { return _instance ?? (_instance = new List<T>()); }
        }
    }
}
