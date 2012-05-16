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

        // ReSharper disable InconsistentNaming
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

        [Test]
        public void Can_calculate_simple_balance_with_all_nulls()
        {
            var balanceService = new BalanceCalculatorService();

            var incomes = new List<Income>();
            incomes.Add(new Income() { AccountID = 1, ActualValue = null });
            incomes.Add(new Income() { AccountID = 1, ActualValue = null });

            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ActualValue = null });
            expenses.Add(new Expense() { AccountID = 1, ActualValue = null });


            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.0m);

        }

        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.00m, ActualValue = 10.20m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.01m, ActualValue = 10.21m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.22m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.23m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.24m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.05m, ActualValue = 10.25m });
            
            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.00m, ActualValue = 10.10m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.01m, ActualValue = 10.11m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.12m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.13m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.14m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.05m, ActualValue = 10.15m });

            var balanceService = new BalanceCalculatorService();
            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.60m);

        }

       
        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values_and_some_nulls()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.00m, ActualValue = null });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.01m, ActualValue = null });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.22m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = null });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.24m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = 10.05m, ActualValue = 10.25m });
            
            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = null, ActualValue = 10.10m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.01m, ActualValue = null});
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.12m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.13m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = null, ActualValue = 10.14m });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.05m, ActualValue = null });

            var balanceService = new BalanceCalculatorService();
            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == -9.83m);

        }

        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values_some_nulls_2()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = 10.20m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = 10.21m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = 10.22m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = 10.23m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = 10.24m });
            incomes.Add(new Income() { AccountID = 1, ExpectedValue = null, ActualValue = 10.25m });

            var expenses = new List<Expense>();
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.00m, ActualValue = null });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.01m, ActualValue = null });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.02m, ActualValue = null });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.03m, ActualValue = null });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.04m, ActualValue = null });
            expenses.Add(new Expense() { AccountID = 1, ExpectedValue = 10.05m, ActualValue = null });

            var balanceService = new BalanceCalculatorService();
            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 1.20m);

        }
        

        public void Can_calculate_balance_with_initial_balance()
        {
        }

        public void Can_calcultate_balance_from_given_date()
        {
        }

    }
}
