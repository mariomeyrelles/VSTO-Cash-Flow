using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculatorService
    {
        public decimal CalculateBalance(int accountId, IEnumerable<Income> incomes, IEnumerable<Expense> expenses, decimal initialBalance = 0.0m)
        {
            var incomesForAccount = incomes.Where(x => x.AccountID == accountId);
            var expensesForAccount = expenses.Where(x => x.AccountID == accountId);

            //first try to calculate the balance.
            var incomeSum = 0.0m;
            var expenseSum = 0.0m;
            incomeSum = IncomeSum(incomeSum, incomesForAccount);

            expenseSum = ExpenseSum(expenseSum, expensesForAccount);

            var balance = incomeSum - expenseSum + initialBalance;
            
            return balance;
        }

        public decimal CalculateBalanceUptoGivenDate(int accountId,IEnumerable<Income> incomes, IEnumerable<Expense> expenses, DateTime maxDate)
        {
            var incomesForAccount = incomes.Where(x => x.AccountID == accountId && x.Date.Today() <= maxDate.Today());
            var expensesForAccount = expenses.Where(x => x.AccountID == accountId && x.Date.Today() <= maxDate.Today());

            //first try to calculate the balance.
            var incomeSum = 0.0m;
            var expenseSum = 0.0m;
            incomeSum = IncomeSum(incomeSum, incomesForAccount);

            expenseSum = ExpenseSum(expenseSum, expensesForAccount);

            var balance = incomeSum - expenseSum ;

            return balance;
        }

        public decimal CalculateBalanceAsOfGivenDate(int accountId, IEnumerable<Income> incomes, IEnumerable<Expense> expenses, DateTime minDate)
        {
            var incomesForAccount = incomes.Where(x => x.AccountID == accountId && x.Date.Today() >= minDate.Today());
            var expensesForAccount = expenses.Where(x => x.AccountID == accountId && x.Date.Today() >= minDate.Today());

            //first try to calculate the balance.
            var incomeSum = 0.0m;
            var expenseSum = 0.0m;
            incomeSum = IncomeSum(incomeSum, incomesForAccount);

            expenseSum = ExpenseSum(expenseSum, expensesForAccount);

            var balance = incomeSum - expenseSum;

            return balance;
        }

        private static decimal ExpenseSum(decimal expenseSum, IEnumerable<Expense> expensesForAccount)
        {
            foreach (var expense in expensesForAccount)
            {
                if (expense.ActualValue.HasValue)
                    expenseSum += expense.ActualValue.Value;
                else
                    expenseSum += expense.ExpectedValue ?? 0.0m;
            }
            return expenseSum;
        }

        private static decimal IncomeSum(decimal incomeSum, IEnumerable<Income> incomesForAccount)
        {
            foreach (var income in incomesForAccount)
            {
                if (income.ActualValue.HasValue)
                    incomeSum += income.ActualValue.Value;
                else
                    incomeSum += income.ExpectedValue ?? 0.0m;
            }
            return incomeSum;
        }
    }
}