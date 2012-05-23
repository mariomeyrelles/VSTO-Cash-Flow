using System;
using System.Collections.Generic;

namespace ModernCashFlow.Domain.Services
{
    //note: first try to model the cash flow calculation result

    public class CashFlow
    {
        public List<CashFlowEntry> Entries { get; set; }
    }

    public class CashFlowEntry
    {
        public int AccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

    }
}