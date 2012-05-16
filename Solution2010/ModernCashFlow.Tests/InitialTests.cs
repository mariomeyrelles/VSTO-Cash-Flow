using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using NUnit.Framework;

namespace ModernCashFlow.Tests
{
    public class InitialTests
    {

        [Test]
        public void Can_calculate_simple_balance()
        {
            var balanceService = new BalanceCalculatorService();

            var incomes = new List<Income>();
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.01m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.02m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.03m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.04m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.05m});
            
            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.01m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.02m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.03m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.04m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.05m});

           
            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.0m);
            
        }

        [Test]
        public void Can_calculate_simple_balance_with_some_nulls()
        {
            var balanceService = new BalanceCalculatorService();

            var incomes = new List<Income>();
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.01m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.02m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = null});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.04m});
            incomes.Add(new Income(){AccountID = 1, ActualValue = 10.05m});
            
            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.01m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.02m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.03m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = 10.04m});
            expenses.Add(new Expense() { AccountID = 1, ActualValue = null});


            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.02m);

        }

       
    }
}
