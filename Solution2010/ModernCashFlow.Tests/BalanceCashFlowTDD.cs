﻿using System;
using System.Collections.Generic;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using NUnit.Framework;
using System.Linq;

namespace ModernCashFlow.Tests
{
    public class BalanceCashFlowTDD
    {
        // ReSharper disable InconsistentNaming
        [Test]
        public void Can_calculate_simple_balance()
        {
            var balanceService = new BalanceCalculationService();

            var incomes = new List<Income>();
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.01m});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.02m});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.03m});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.04m});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.05m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.01m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.02m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.03m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.04m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.05m});

            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0.0m);
        }

        [Test]
        public void Can_calculate_simple_balance_with_some_nulls()
        {
            var balanceService = new BalanceCalculationService();

            var incomes = new List<Income>();
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.01m});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.02m});
            incomes.Add(new Income {AccountId = 1, ActualValue = null});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.04m});
            incomes.Add(new Income {AccountId = 1, ActualValue = 10.05m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.01m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.02m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.03m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = 10.04m});
            expenses.Add(new Expense {AccountId = 1, ActualValue = null});

            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0.02m);
        }

        [Test]
        public void Can_calculate_simple_balance_with_all_nulls()
        {
            var balanceService = new BalanceCalculationService();

            var incomes = new List<Income>();
            incomes.Add(new Income {AccountId = 1, ActualValue = null});
            incomes.Add(new Income {AccountId = 1, ActualValue = null});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountId = 1, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ActualValue = null});


            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0.0m);
        }

        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.00m, ActualValue = 10.20m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.01m, ActualValue = 10.21m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.02m, ActualValue = 10.22m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.03m, ActualValue = 10.23m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.04m, ActualValue = 10.24m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.05m, ActualValue = 10.25m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.00m, ActualValue = 10.10m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.01m, ActualValue = 10.11m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.02m, ActualValue = 10.12m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.03m, ActualValue = 10.13m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.04m, ActualValue = 10.14m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.05m, ActualValue = 10.15m});

            var balanceService = new BalanceCalculationService();

            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0.60m);
        }


        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values_and_some_nulls()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.00m, ActualValue = null});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.01m, ActualValue = null});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.02m, ActualValue = 10.22m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = null});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.04m, ActualValue = 10.24m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = 10.05m, ActualValue = 10.25m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = null, ActualValue = 10.10m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.01m, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.02m, ActualValue = 10.12m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.03m, ActualValue = 10.13m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = null, ActualValue = 10.14m});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.05m, ActualValue = null});

            var balanceService = new BalanceCalculationService();
            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == -9.83m);
        }

        [Test]
        public void Can_calculate_balance_considering_actual_and_expected_values_some_nulls_2()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = 10.20m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = 10.21m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = 10.22m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = 10.23m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = 10.24m});
            incomes.Add(new Income {AccountId = 1, ExpectedValue = null, ActualValue = 10.25m});

            var expenses = new List<Expense>();
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.00m, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.01m, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.02m, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.03m, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.04m, ActualValue = null});
            expenses.Add(new Expense {AccountId = 1, ExpectedValue = 10.05m, ActualValue = null});

            var balanceService = new BalanceCalculationService();
            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 1.20m);
        }

        [Test]
        public void Can_calculate_balance_with_initial_balance()
        {
            var initialBalance = 1.19m;

            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 10.00m, ActualValue = 10.10m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 10.01m, ActualValue = 10.11m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 10.02m, ActualValue = 10.12m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.13m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 10.04m, ActualValue = 10.14m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 10.05m, ActualValue = null });

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 10.00m, ActualValue = 10.20m });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 10.01m, ActualValue = 10.21m });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 10.02m, ActualValue = 10.22m });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 10.03m, ActualValue = 10.23m });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 10.04m, ActualValue = 10.24m });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = null, ActualValue = null });

            var balanceService = new BalanceCalculationService();
            var calculationArgs = new CalculationArgs(incomes, expenses){InitialBalance = initialBalance};
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 10.74m);
        }

        [Test]
        public void Can_calcultate_balance_up_to_given_date()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012,01,01)});
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });
            

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012,01,02) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012,02,03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012,03,03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012,04,03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012,05,03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012,06,05) });
            
            var balanceService = new BalanceCalculationService();

            var balance = balanceService.CalculateBalance(new CalculationArgs(incomes,expenses){EndingDate = new DateTime(2011, 01, 31)});
            Console.WriteLine("balance (before january): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0);

            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2012, 01, 31) });
            Console.WriteLine("balance (end of january): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == -99.9m);
            
            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2012, 02, 28) });
            Console.WriteLine("balance (end of february): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == -300.00m);

            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2012, 03, 01) });
            Console.WriteLine("balance (begining of march): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 1200.00m);
            
            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2012, 03, 15) });
            Console.WriteLine("balance (middle of march): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 69.79m);
            
            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2012, 05, 15) });
            Console.WriteLine("balance (middle of may): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 669.64m);
            
            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2013, 01, 01) });
            Console.WriteLine("balance (jan 2013): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 949.42m);

        } 
        

        [Test]
        public void Can_calcultate_balance_up_to_given_date_and_time_ignoring_times()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012,05,01,8,0,0)});
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012,05, 01,10,0,0) });
         

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012,05,01,9,0,0) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1500.11m, Date = new DateTime(2012,05,01,11,0,0) });
            
            var balanceService = new BalanceCalculationService();


            var balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2011, 01, 01) });
            Console.WriteLine("balance (at 00:00): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0);


            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2011, 01, 01, 08, 01, 0) });
            Console.WriteLine("balance (at 08:01): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0);
            
            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2013, 01, 01, 09, 0, 0) });
            Console.WriteLine("balance (at 09:00): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0);


            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { EndingDate = new DateTime(2013, 01, 01, 12, 0, 0) });
            Console.WriteLine("balance (at 12:00): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0);
        } 

        [Test]
        public void Can_calcultate_balance_as_of_given_date()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 01, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });


            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 01, 02) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012, 02, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012, 03, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 05, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012, 06, 05) });

            var balanceService = new BalanceCalculationService();

            var balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses) { StartingDate = new DateTime(2013, 01, 31) });
            Console.WriteLine("balance (starting april): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 0);
        }

        [Test]
        public void Can_calcultate_balance_of_a_given_date_interval()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 01, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });


            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 01, 02) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012, 02, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012, 03, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 05, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012, 06, 05) });

            var balanceService = new BalanceCalculationService();

            var balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses)
                                                                    {
                                                                        StartingDate = new DateTime(2012, 03, 1),
                                                                        EndingDate = new DateTime(2012, 03, 31)
                                                                    });
            Console.WriteLine("balance (only march): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 369.79m);


            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 1),
                EndingDate = new DateTime(2012, 04, 30)
            });
            Console.WriteLine("balance (only march and april): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 669.71m);


            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 01, 1),
                EndingDate = new DateTime(2012, 07, 1)
            });
            Console.WriteLine("balance (jan to jun): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 8930.48m - 7981.06m);


        }
        


        [Test]
        public void Can_calcultate_balance_of_given_date_interval_with_initial_balance()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 01, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });


            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 01, 02) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1700.21m, Date = new DateTime(2012, 02, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1130.21m, Date = new DateTime(2012, 03, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.01m, ActualValue = 1200.21m, Date = new DateTime(2012, 05, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1500.02m, ActualValue = 1150.22m, Date = new DateTime(2012, 06, 05) });

            var balanceService = new BalanceCalculationService();

            var balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 1),
                EndingDate = new DateTime(2012, 03, 31),
                InitialBalance = 717.12m
            });
            Console.WriteLine("balance (only march): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 369.79m + 717.12m);


            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 1),
                EndingDate = new DateTime(2012, 04, 30),
                InitialBalance = -12.23m
            });
            Console.WriteLine("balance (only march and april): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == -12.23m + 669.71m);


            balance = balanceService.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 01, 1),
                EndingDate = new DateTime(2012, 07, 1),
                InitialBalance = 814.35m
            });
            Console.WriteLine("balance (jan to jun): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 814.35m + 8930.48m - 7981.06m);
            
        }

        [Test]
        public void Can_calculate_balance_using_only_incomes()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 01, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 02, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 03, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 05, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 06, 04) });

            var balanceService = new BalanceCalculationService();


            var balance = balanceService.CalculateBalance(new CalculationArgs(incomes, null)
            {
                StartingDate = new DateTime(2012, 03, 1),
                EndingDate = new DateTime(2012, 03, 31),
                InitialBalance = 717.12m
            });
            Console.WriteLine("balance (only march): " + balance.ForAccountId(1));
            Assert.IsTrue(balance.ForAccountId(1) == 717.12m + 1500);
        }

        [Test]
        public void Can_calculate_simple_cashflow_for_one_account()
        {
            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 04, 05) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 04, 05) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1250.00m, Date = new DateTime(2012, 04, 07) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 04, 02) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 04, 03) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 04) });
            

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 03, 31) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1700.21m, Date = new DateTime(2012, 04, 01) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1300.00m, ActualValue = null,     Date = new DateTime(2012, 04, 05) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = null,     ActualValue = 1150.22m, Date = new DateTime(2012, 04, 06) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1130.21m, Date = new DateTime(2012, 04, 02) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1300.81m, Date = new DateTime(2012, 04, 04) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1150.22m, Date = new DateTime(2012, 04, 05) });
            var service = new BalanceCalculationService();

            var cashflow = service.CalculateCashflow(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 30),
                EndingDate = new DateTime(2012, 04, 08)
            });

            Assert.IsTrue(cashflow.At(new DateTime(2012,03,31),1).Value==-1600.0m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,01),1).Value==-1800.11m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,02),1).Value==-1430.21m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,03),1).Value==-1130.42m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,04),1).Value==-931.10m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,05),1).Value==-451.18m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,06),1).Value==-1601.40m);
            Assert.IsTrue(cashflow.At(new DateTime(2012,04,07),1).Value==-351.40m);


            var balance1 = service.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 30),
                EndingDate = new DateTime(2012, 04, 08)
            });

            Assert.IsTrue(balance1.ForAccountId(1) == -351.40m);
            
           
            //test smaller period
            cashflow = service.CalculateCashflow(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 04, 02),
                EndingDate = new DateTime(2012, 04, 04)
            });

            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 02), 1).Value == 369.90m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 03), 1).Value == 669.69m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 04), 1).Value == 869.01m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 05), 1).Value == 869.01m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 05, 05), 1).Value == 869.01m);
            
            var balance2 = service.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 04, 02),
                EndingDate = new DateTime(2012, 04, 04)
            });

            Assert.IsTrue(balance2.ForAccountId(1) == 869.01m);

            
        }

        [Test]
        public void Can_calculate_simple_balance_with_more_than_one_account_and_some_nulls()
        {

            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.20m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.21m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.22m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.23m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.24m });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 10.25m });

            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 10.00m, ActualValue = null });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 10.01m, ActualValue = null });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 10.02m, ActualValue = null });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 10.03m, ActualValue = null });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 10.04m, ActualValue = null });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 10.05m, ActualValue = null });

            var balanceService = new BalanceCalculationService();
            var calculationArgs = new CalculationArgs(incomes, expenses);
            var balance = balanceService.CalculateBalance(calculationArgs);

            Console.WriteLine("balance: " + balance.ForAccountId(1));
            Console.WriteLine("balance: " + balance.ForAccountId(4));
            Assert.IsTrue(balance.ForAccountId(1) == 61.35m);
            Assert.IsTrue(balance.ForAccountId(2) == 0);
            Assert.IsTrue(balance.ForAccountId(3) == 0);
            Assert.IsTrue(balance.ForAccountId(4) == -60.15m);
        }

        [Test]
        public void Can_calculate_simple_cashflow_with_more_than_one_account()
        {
            #region Scenario

            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.10m, Date = new DateTime(2012, 04, 01) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1500.14m, Date = new DateTime(2012, 04, 05) });
            incomes.Add(new Income { AccountId = 4, ExpectedValue = 1500.00m, ActualValue = 1430.00m, Date = new DateTime(2012, 04, 05) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = 1500.00m, ActualValue = 1250.00m, Date = new DateTime(2012, 04, 07) });
            incomes.Add(new Income { AccountId = 4, ExpectedValue = 1500.00m, ActualValue = 1500.11m, Date = new DateTime(2012, 04, 02) });
            incomes.Add(new Income { AccountId = 3, ExpectedValue = 1500.00m, ActualValue = null, Date = new DateTime(2012, 04, 03) });
            incomes.Add(new Income { AccountId = 1, ExpectedValue = null, ActualValue = 1500.13m, Date = new DateTime(2012, 04, 04) });


            var expenses = new List<Expense>();
            expenses.Add(new Expense { AccountId = 3, ExpectedValue = 1450.00m, ActualValue = 1600.00m, Date = new DateTime(2012, 03, 31) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1700.21m, Date = new DateTime(2012, 04, 01) });
            expenses.Add(new Expense { AccountId = 3, ExpectedValue = 1300.00m, ActualValue = null, Date = new DateTime(2012, 04, 05) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = null, ActualValue = 1150.22m, Date = new DateTime(2012, 04, 06) });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 1450.00m, ActualValue = 1130.21m, Date = new DateTime(2012, 04, 02) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1200.21m, Date = new DateTime(2012, 04, 03) });
            expenses.Add(new Expense { AccountId = 4, ExpectedValue = 1450.00m, ActualValue = 1300.81m, Date = new DateTime(2012, 04, 04) });
            expenses.Add(new Expense { AccountId = 1, ExpectedValue = 1450.00m, ActualValue = 1150.22m, Date = new DateTime(2012, 04, 05) });

            #endregion

            var service = new BalanceCalculationService();

            var cashflow = service.CalculateCashflow(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 30),
                EndingDate = new DateTime(2012, 04, 08)
            });

           var balance1 = service.CalculateBalance(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 03, 30),
                EndingDate = new DateTime(2012, 04, 08)
            });

            Assert.IsTrue(balance1.ForAccountId(1) == 549.51m);
            Assert.IsTrue(balance1.ForAccountId(3) == -1400m);
            Assert.IsTrue(balance1.ForAccountId(4) == 499.09m);

            //cash flows per date and accountID = 1
            Assert.IsTrue(cashflow.At(new DateTime(2012, 03, 31), 1).Value == 0.0m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 01), 1).Value == -200.11m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 02), 1).Value == -200.11m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 03), 1).Value == -1400.32m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 04), 1).Value == 99.81m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 05), 1).Value == 449.73m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 06), 1).Value == -700.49m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 07), 1).Value == 549.51m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 08), 1).Value == 549.51m);
            
            //cash flows per date and accountID = 3
            Assert.IsTrue(cashflow.At(new DateTime(2012, 03, 31), 3).Value == -1600m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 01), 3).Value == -1600m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 02), 3).Value == -1600m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 03), 3).Value == -100.00m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 04), 3).Value == -100.00000000m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 05), 3).Value == -1400m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 06), 3).Value == -1400m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 07), 3).Value == -1400m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 08), 3).Value == -1400m);

            //cash flows per date and accountID = 4
            Assert.IsTrue(cashflow.At(new DateTime(2012, 03, 31), 4).Value == 00000m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 01), 4).Value == 0m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 02), 4).Value == 369.90m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 03), 4).Value == 369.9m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 04), 4).Value == -930.91m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 05), 4).Value == 499.09m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 06), 4).Value == 499.09m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 07), 4).Value == 499.09m);
            Assert.IsTrue(cashflow.At(new DateTime(2012, 04, 08), 4).Value == 499.09m);


            //test with different intervals
            var cashflow2 = service.CalculateCashflow(new CalculationArgs(incomes, expenses)
            {
                StartingDate = new DateTime(2012, 04, 02),
                EndingDate = new DateTime(2012, 04, 05)
            });
            

            Assert.IsTrue(cashflow2.At(new DateTime(2012, 04, 01), 1).Value == 0);
            Assert.IsTrue(cashflow2.At(new DateTime(2012, 04, 02), 1).Value == 0);
            Assert.IsTrue(cashflow2.At(new DateTime(2012, 04, 03), 1).Value == -1200.21m);
            Assert.IsTrue(cashflow2.At(new DateTime(2012, 04, 04), 1).Value == 299.92m);
            Assert.IsTrue(cashflow2.At(new DateTime(2012, 04, 05), 1).Value == 649.84m);
            Assert.IsTrue(cashflow2.At(new DateTime(2012, 04, 06), 1).Value == 649.84m);


        }

    }
}