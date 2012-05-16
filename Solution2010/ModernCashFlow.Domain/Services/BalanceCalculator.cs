using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculatorService
    {
        public decimal CalculateBalance(int accountID, IEnumerable<Income> incomes, IEnumerable<Expense> expenses)
        {
            //first try to calculate the balance.
            var incomeSum = 0.0m;
            var expenseSum = 0.0m;
            foreach (var income in incomes.Where(x => x.AccountID == accountID))
            {
                if (income.ActualValue.HasValue)
                    incomeSum += income.ActualValue.Value;
                else
                    incomeSum += income.ExpectedValue ?? 0.0m;
            }

            foreach (var expense in expenses.Where(x => x.AccountID == accountID))
            {
                if (expense.ActualValue.HasValue)
                    expenseSum += expense.ActualValue.Value;
                else
                    expenseSum += expense.ExpectedValue ?? 0.0m;
            }

            var balance = incomeSum - expenseSum;

            
            return balance;
        }
    }
}