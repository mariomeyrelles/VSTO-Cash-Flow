﻿using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class BalanceCalculationService
    {
        public BalanceCalculationResult CalculateBalance(CalculationArgs args)
        {
            var balanceResult = new BalanceCalculationResult();
            var accounts = args.GetDistinctAccountIds();
            foreach (var accountId in accounts)
            {
                var id = accountId;
                var incomes = args.Incomes.Where(x => x.AccountId == id);
                var expenses = args.Expenses.Where(x => x.AccountId == id);

                if (args.StartingDate.HasValue)
                {
                    incomes = incomes.Where(x => x.Date >= args.StartingDate);
                    expenses = expenses.Where(x => x.Date >= args.StartingDate);
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
                
                balanceResult.AddEntry(accountId,balance);
                

            }
            return balanceResult;
        }


        private decimal ExpenseSum(decimal expenseSum, IEnumerable<Expense> expensesForAccount)
        {
            expenseSum += expensesForAccount.Sum(expense => expense.Value);
            return expenseSum;
        }

        private decimal IncomeSum(decimal incomeSum, IEnumerable<Income> incomesForAccount)
        {
            incomeSum += incomesForAccount.Sum(income => income.Value);
            return incomeSum;
        }


        public CashFlowCalculationResult CalculateCashflow(CalculationArgs args)
        {
            var transactions = new List<IMoneyTransaction>();
            transactions.AddRange(args.Incomes.Where(x => x.Date.HasValue));
            transactions.AddRange(args.Expenses.Where(x => x.Date.HasValue));

            decimal runningSum = 0;

            var dailySums = (from x in transactions.OrderBy(t=>t.Date)
                             group x by new { x.Date, AccountID = x.AccountId}
                             into g
                             select new {Date = g.Key.Date, AccountId = g.Key.AccountID, DailyAmount = g.Sum(x => x.Value)}).ToList();
         
            var cashFlowCalc = dailySums
                .OrderBy(x => x.Date)
                .Select(x =>{
                                runningSum += x.DailyAmount;
                                return new CashFlowEntry { Date = x.Date.Value, AccountId = x.AccountId, Value = runningSum};
                            }
                        );

            var cashFlow = new CashFlowCalculationResult(cashFlowCalc.ToList());
            return cashFlow;

        }
    }

    
}