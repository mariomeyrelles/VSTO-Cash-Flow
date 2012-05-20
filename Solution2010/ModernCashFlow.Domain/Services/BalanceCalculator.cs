﻿using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculatorService
    {
        public decimal CalculateBalance(BalanceCalcArgs args)
        {
            var incomes = args.Incomes;
            var expenses = args.Expenses;

            if (args.StartingDate.HasValue)
            {
                incomes = incomes.Where(x => x.Date >= args.StartingDate);
                expenses =  expenses.Where(x => x.Date >= args.StartingDate);
            }

            if (args.EndingDate.HasValue)
            {
                incomes = incomes.Where(x => x.Date.Today() <= args.EndingDate);
                expenses = expenses.Where(x => x.Date.Today() <= args.EndingDate);
            }
            
            var incomeSum = 0.0m;
            var expenseSum = 0.0m;

            incomeSum = IncomeSum(incomeSum, incomes);
            expenseSum = ExpenseSum(expenseSum, expenses);

            var balance = incomeSum - expenseSum + args.InitialBalance;
            
            return balance;
        }


        private decimal ExpenseSum(decimal expenseSum, IEnumerable<Expense> expensesForAccount)
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

        private decimal IncomeSum(decimal incomeSum, IEnumerable<Income> incomesForAccount)
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