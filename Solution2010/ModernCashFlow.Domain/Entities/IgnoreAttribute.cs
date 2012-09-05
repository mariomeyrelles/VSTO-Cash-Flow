using System;

namespace ModernCashFlow.Domain.Entities
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited = false)]
    public sealed class IgnoreAttribute : Attribute
    {
        private readonly string _reason;

        public IgnoreAttribute(string reason)
        {
            _reason = reason;
        }
    }
}