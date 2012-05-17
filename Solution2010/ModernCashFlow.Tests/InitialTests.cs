using System;
using System.Collections.Generic;
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
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.01m});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.02m});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.03m});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.04m});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.05m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.01m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.02m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.03m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.04m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.05m});


            decimal balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.0m);
        }

        [Test]
        public void Can_calculate_simple_balance_with_some_nulls()
        {
            var balanceService = new BalanceCalculatorService();

            var incomes = new List<Income>();
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.01m});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.02m});
            incomes.Add(new Income {AccountID = 1, ActualValue = null});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.04m});
            incomes.Add(new Income {AccountID = 1, ActualValue = 10.05m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.01m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.02m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.03m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = 10.04m});
            expenses.Add(new Expense {AccountID = 1, ActualValue = null});


            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.02m);
        }

        [Test]
        public void Can_calculate_simple_balance_with_all_nulls()
        {
            var balanceService = new BalanceCalculatorService();

            var incomes = new List<Income>();
            incomes.Add(new Income {AccountID = 1, ActualValue = null});
            incomes.Add(new Income {AccountID = 1, ActualValue = null});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountID = 1, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ActualValue = null});


            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.0m);
        }

        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.00m, ActualValue = 10.20m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.01m, ActualValue = 10.21m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.22m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.23m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.24m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.05m, ActualValue = 10.25m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.00m, ActualValue = 10.10m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.01m, ActualValue = 10.11m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.12m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.13m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.14m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.05m, ActualValue = 10.15m});

            var balanceService = new BalanceCalculatorService();
            decimal balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 0.60m);
        }


        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values_and_some_nulls()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.00m, ActualValue = null});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.01m, ActualValue = null});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.22m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = null});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.24m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = 10.05m, ActualValue = 10.25m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = null, ActualValue = 10.10m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.01m, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.12m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.13m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = null, ActualValue = 10.14m});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.05m, ActualValue = null});

            var balanceService = new BalanceCalculatorService();
            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == -9.83m);
        }

        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values_some_nulls_2()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = 10.20m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = 10.21m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = 10.22m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = 10.23m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = 10.24m});
            incomes.Add(new Income {AccountID = 1, ExpectedValue = null, ActualValue = 10.25m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.00m, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.01m, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.02m, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.03m, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.04m, ActualValue = null});
            expenses.Add(new Expense {AccountID = 1, ExpectedValue = 10.05m, ActualValue = null});

            var balanceService = new BalanceCalculatorService();
            var balance = balanceService.CalculateBalance(1, incomes, expenses);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 1.20m);
        }

        [Test]
        public void Can_calculate_balance_with_initial_balance()
        {
            var initialBalance = 1.19m;

            var incomes = new List<Income>();
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 10.00m, ActualValue = 10.10m });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 10.01m, ActualValue = 10.11m });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.12m });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = null, ActualValue = 10.13m });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.14m });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 10.05m, ActualValue = null });

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 10.00m, ActualValue = 10.20m });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 10.01m, ActualValue = 10.21m });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 10.02m, ActualValue = 10.22m });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 10.03m, ActualValue = 10.23m });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 10.04m, ActualValue = 10.24m });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = null, ActualValue = null });

            var balanceService = new BalanceCalculatorService();
            var balance = balanceService.CalculateBalance(1, incomes, expenses, initialBalance);

            Console.WriteLine("balance: " + balance);
            Assert.IsTrue(balance == 10.74m);
        }

        [Test]
        public void Can_calcultate_balance_at_given_date()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012,01,01)});
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });
            

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012,01,02) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012,02,03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012,03,03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012,04,03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012,05,03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012,06,05) });
            
            var balanceService = new BalanceCalculatorService();

            var balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 31));
            Console.WriteLine("balance (before january): " + balance);
            Assert.IsTrue(balance == 0);
            
            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2012, 01, 31));
            Console.WriteLine("balance (end of january): " + balance);
            Assert.IsTrue(balance == -99.9m);
            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2012, 02, 28));
            Console.WriteLine("balance (end of february): " + balance);
            Assert.IsTrue(balance == -300.00m);


            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2012, 03, 1));
            Console.WriteLine("balance (begining of march): " + balance);
            Assert.IsTrue(balance == 1200.00m);

            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2012, 03, 15));
            Console.WriteLine("balance (middle of march): " + balance);
            Assert.IsTrue(balance == 69.79m);
            
            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2012, 05, 15));
            Console.WriteLine("balance (middle of may): " + balance);
            Assert.IsTrue(balance == 669.64m);

            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2013, 01, 1));
            Console.WriteLine("balance (jan 2013): " + balance);
            Assert.IsTrue(balance == 949.42m);

        } 
        

        [Test]
        public void Can_calcultate_balance_at_given_date_and_time_ignoring_times()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012,05,01,8,0,0)});
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012,05, 01,10,0,0) });
         

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012,05,01,9,0,0) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1500.11m, Date = new DateTime(2012,05,01,11,0,0) });
            
            var balanceService = new BalanceCalculatorService();

            var balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 01));
            Console.WriteLine("balance (at 00:00): " + balance);
            Assert.IsTrue(balance == 0); 
            
            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 01,08,1,0));
            Console.WriteLine("balance (at 08:01): " + balance);
            Assert.IsTrue(balance == 0);


            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 01,09,0,0));
            Console.WriteLine("balance (at 09:00): " + balance);
            Assert.IsTrue(balance == 0);  
            
            balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 01,12,0,0));
            Console.WriteLine("balance (at 12:00): " + balance);
            Assert.IsTrue(balance == 0);
        } 

        public void Can_calcultate_balance_as_of_given_date()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 01, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });


            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 01, 02) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012, 02, 03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012, 03, 03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 05, 03) });
            expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012, 06, 05) });

            var balanceService = new BalanceCalculatorService();

            var balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 31));
            Console.WriteLine("balance (starting april): " + balance);
            Assert.IsTrue(balance == 0);
        }

        public void Can_calcultate_balance_as_of_given_date_interval()
        {
            //var incomes = new List<Income>();
            //incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 01, 01) });
            //incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            //incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            //incomes.Add(new Income { AccountID = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            //incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            //incomes.Add(new Income { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });


            //var expenses = new List<Expense>();
            //expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 01, 02) });
            //expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012, 02, 03) });
            //expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012, 03, 03) });
            //expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            //expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 05, 03) });
            //expenses.Add(new Expense { AccountID = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012, 06, 05) });

            //var balanceService = new BalanceCalculatorService();

            //var balance = balanceService.CalculateBalanceUptoGivenDate(1, incomes, expenses, new DateTime(2011, 01, 31));
            //Console.WriteLine("balance (from may): " + balance);
            //Assert.IsTrue(balance == 0);
        }
        



        public void Can_calcultate_balance_as_of_given_date_with_initial_balance()
        {
        }


    }
}