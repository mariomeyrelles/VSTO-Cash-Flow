using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculatorService
    {
        public decimal? CalculateBalance(int accountID, IEnumerable<Income> incomes, IEnumerable<Expense> expenses)
        {
            //first try to calculate the balance.
            var balance = incomes.Where(x => x.AccountID == accountID).Sum(x => x.ActualValue) -
                             expenses.Where(x => x.AccountID == accountID).Sum(x => x.ActualValue);

            return balance;
        }
    }
}