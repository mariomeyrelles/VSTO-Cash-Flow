using System;

namespace ModernCashFlow.Domain.Services
{
    public struct CashFlowEntry
    {
        public int AccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

    }
}