using System.Collections.Generic;

namespace ModernCashFlow.Domain.Services
{
    //note: first try to model the cash flow calculation result

    public class CashFlow
    {
        public List<CashFlowEntry> Entries { get; set; }
    }
}