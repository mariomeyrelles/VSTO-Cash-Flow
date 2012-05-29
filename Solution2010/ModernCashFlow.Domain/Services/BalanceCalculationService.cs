using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculationService
    {
        public decimal CalculateSimpleBalance(CalculationArgs args)
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

            var balance = incomeSum + expenseSum + args.InitialBalance;
            
            return balance;
        }


        private decimal ExpenseSum(decimal expenseSum, IEnumerable<Expense> expensesForAccount)
        {
            foreach (var expense in expensesForAccount)
            {
                expenseSum += expense.Value;
            }
            return expenseSum;
        }

        private decimal IncomeSum(decimal incomeSum, IEnumerable<Income> incomesForAccount)
        {
            foreach (var income in incomesForAccount)
            {
                incomeSum += income.Value;
            }
            return incomeSum;
        }


        public CashFlow CalculateCashflow(CalculationArgs args)
        {
            var transactions = new List<IMoneyTransaction>();
            transactions.AddRange(args.Incomes.Where(x => x.Date.HasValue));
            transactions.AddRange(args.Expenses.Where(x => x.Date.HasValue));

            //sort by date ascending to organize transactions.
// ReSharper disable PossibleInvalidOperationException
            transactions.Sort((t1, t2) => t1.Date.Value.CompareTo(t2.Date.Value));
// ReSharper restore PossibleInvalidOperationException


            decimal runningSum = 0;

            var dailySums = (from x in transactions
                             group x by new { x.Date, x.AccountID}
                             into g
                             select new {Date = g.Key.Date, AccountId = g.Key.AccountID, DailyAmount = g.Sum(x => x.Value)}).ToList();

         
            var query = dailySums
                .OrderBy(x => x.Date)
                .Select(x =>
                            {
                                runningSum += x.DailyAmount;
                                return new CashFlowEntry
                                           {
                                               Date = x.Date.Value,
                                               AccountId = x.AccountId,
                                               Value = runningSum
                                           };
                            }
                );


            var cashFlow = new CashFlow {Entries = query.ToList()};
            return cashFlow;

        }
    }

    
}