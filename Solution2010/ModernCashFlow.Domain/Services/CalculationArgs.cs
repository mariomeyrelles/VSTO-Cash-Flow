using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class CalculationArgs
    {
        private DateTime? _endingDate;
        private DateTime? _startingDate;

        public CalculationArgs(IEnumerable<Income> incomes, IEnumerable<Expense> expenses)
        {
            Incomes = new List<Income>();
            Expenses = new List<Expense>();

            if (incomes != null) Incomes = incomes.ToList();
            if (expenses != null) Expenses = expenses.ToList();
        }

        public IEnumerable<Income> Incomes { get; private set; }
        public IEnumerable<Expense> Expenses { get; private set; }

        public DateTime StartingDate
        {
            get { return _startingDate.Today(DateTime.MinValue); }
            set { _startingDate = value; }
        }

        public DateTime EndingDate
        {
            get { return _endingDate.Today(DateTime.MaxValue); }
            set { _endingDate = value; }
        }

        public decimal InitialBalance { get; set; }


        public int[] GetDistinctAccountIds()
        {
            var result = Incomes.Select(x => x.AccountId).Union(Expenses.Select(x=>x.AccountId));
            return result.ToArray();

        }
    }
}