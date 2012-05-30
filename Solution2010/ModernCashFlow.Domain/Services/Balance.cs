using System;
using System.Linq;
using System.Text;

namespace ModernCashFlow.Domain.Services
{
    public struct BalanceEntry
    {
        public int AccountId { get; set; }
        public decimal Value { get; set; }
    }
}
