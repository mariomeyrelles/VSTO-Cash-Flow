using System;
using System.Collections.Generic;

namespace ModernCashFlow.Domain.ApplicationServices
{
    public class Singleton<T> where T: class, new()
    {
        private static T _instance;

        
        public static T Instance
        {
            get
            {
                return _instance ?? (_instance = new T());
            }
        }
    }
}