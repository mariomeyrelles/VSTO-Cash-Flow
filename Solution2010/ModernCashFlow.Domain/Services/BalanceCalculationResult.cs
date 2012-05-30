using System.Collections.Generic;
using System.Linq;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculationResult
    {
        public BalanceCalculationResult()
        {
            Entries = new List<BalanceEntry>();
        }

        public BalanceCalculationResult(IEnumerable<BalanceEntry> entries)
        {
            Entries = entries.ToList();
        }


        public List<BalanceEntry> Entries { get; private set; }

        public void AddEntry(int accountId, decimal value)
        {
            var entry = new BalanceEntry {AccountId = accountId, Value = value};
            Entries.Add(entry);

        }

        public decimal ForAccountId(int accountId)
        {
            var result = Entries.FirstOrDefault(x => x.AccountId == accountId).Value;
            return result;

        }
    }
}