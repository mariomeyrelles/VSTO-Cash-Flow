using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculationArgs
    {
        public BalanceCalculationArgs(int accountId, IEnumerable<Income> incomes, IEnumerable<Expense> expenses)
        {
        
            Incomes = incomes.Where(x=>x.AccountID == accountId).ToList();
            Expenses = expenses.Where(x => x.AccountID == accountId).ToList();
        }
        
        public IEnumerable<Income> Incomes { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
        
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public decimal InitialBalance { get; set; }
        
    }
}