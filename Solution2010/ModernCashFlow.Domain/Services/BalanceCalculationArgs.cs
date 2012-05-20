using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalcArgs
    {
        public BalanceCalcArgs(int accountId, IEnumerable<Income> incomes, IEnumerable<Expense> expenses)
        {
            Incomes = new List<Income>();
            Expenses = new List<Expense>();

            if (incomes != null) Incomes = incomes.Where(x=>x.AccountID == accountId).ToList();
            
            if (expenses != null) Expenses = expenses.Where(x => x.AccountID == accountId).ToList();
        }

        public IEnumerable<Income> Incomes { get; private set; }
        public IEnumerable<Expense> Expenses { get; private set; }

        private DateTime? _startingDate;
        public DateTime? StartingDate
        {
            get { return _startingDate.Today(); }
            set { _startingDate = value; }
        }

        private DateTime? _endingDate;
        public DateTime? EndingDate
        {
            get { return _endingDate.Today(); }
            set { _endingDate = value; }
        }

        public decimal InitialBalance { get; set; }
        
    }
}