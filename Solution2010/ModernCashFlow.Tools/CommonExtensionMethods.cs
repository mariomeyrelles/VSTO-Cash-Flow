using System;
using System.Collections.Generic;

namespace ModernCashFlow.Tools
{
    public static class CommonExtensionMethods
    {
        public static void Set<T,U>(this Dictionary<T,U> dictionary,T key, U value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key,value);
                return;
            }
            
            dictionary[key] = value;    
            
            
        }

    }
}