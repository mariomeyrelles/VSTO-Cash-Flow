using System;

namespace ModernCashFlow.Tools
{
    /// <summary>
    /// When used in a domain object property, this columns is considered a resource key for globalization scenarios. So, you can use
    /// worksheet column names in different languages.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class LocalizableColumnNameAttribute : Attribute
    {
        
    }
}