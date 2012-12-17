using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class SummaryCalculationService
    {
        public decimal CalculateIncomesForCurrentMonth(IEnumerable<BaseTransaction> transactions)
        {
            var incomes = transactions.OfType<Income>();
            var today = SystemTime.Now;

            var sumOfIncomes = incomes.Where(x => x.Date.Value.Month == today().Month).Sum(x => x.Value);

            return sumOfIncomes;

        }

        public decimal CalculateIncomesForCurrentMonthUpToGivenDate(IEnumerable<BaseTransaction> transactions, DateTime now)
        {
            var incomes = transactions.OfType<Income>();
          
            var sumOfIncomes = incomes.Where(x => x.Date.Value.Month == now.Month && x.Date <= now).Sum(x => x.Value);

            return sumOfIncomes;
        }
    }
}