using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    //note: first try to model the cash flow calculation result

    public class CashFlowCalculationResult
    {
        public List<CashFlowEntry> Entries { get; private set; }


        public CashFlowCalculationResult()
        {
            Entries = new List<CashFlowEntry>();
        }

        public CashFlowCalculationResult(IEnumerable<CashFlowEntry> entries)
        {
            Entries = entries.ToList();
        }

        public void AddEntry(int accountId, DateTime date, decimal  value)
        {
            var entry = new CashFlowEntry {AccountId = accountId, Date = date, Value = value};
            Entries.Add(entry);
        }

        public void AddEntry(CashFlowEntry entry)
        {
            Entries.Add(entry);
        }

        public void AddEntries(IEnumerable<CashFlowEntry> cashFlowEntries)
        {
            Entries.AddRange(cashFlowEntries);
        }


        public CashFlowEntry At(DateTime date, int accountId)
        {
            var result = this.Entries.Where(x=>x.AccountId == accountId && x.Date.Today() <= date.Today()).LastOrDefault();

            return result;

        }


       
    }

   
}