using System;
using System.Collections.Generic;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Tools;
using NUnit.Framework;

namespace ModernCashFlow.Tests
{
    public class SummaryCalculationsTest
    {
        [Test]
        public void Can_Sum_Incomes_Whole_Month()
        {
            var svc = new SummaryCalculationService();

            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.01m, Date = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 01) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.02m, Date = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 05) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.03m, Date = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 10) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.04m, Date = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 15) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.05m, Date = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 25) });

            decimal incomesForTheMonth = 0;

            SystemTime.Now = () => new DateTime(2012, 12, 15);
            incomesForTheMonth = svc.CalculateIncomesForCurrentMonth(incomes);
            Assert.IsTrue(incomesForTheMonth == 50.15m );

            SystemTime.Now = () => new DateTime(2012, 11, 15);
            incomesForTheMonth = svc.CalculateIncomesForCurrentMonth(incomes);
            Assert.IsTrue(incomesForTheMonth == 0m);

            SystemTime.Now = () => new DateTime(2013, 01, 15);
            incomesForTheMonth = svc.CalculateIncomesForCurrentMonth(incomes);
            Assert.IsTrue(incomesForTheMonth == 0m);


        }

        [Test]
        public void Can_Sum_Incomes_Up_To_Given_Date_In_Current_Month()
        {
            var svc = new SummaryCalculationService();
            SystemTime.Now = () => new DateTime(2012,12,15);


            var incomes = new List<Income>();
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.01m, Date = new DateTime(2012, 12, 02) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.02m, Date = new DateTime(2012, 12, 05) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.03m, Date = new DateTime(2012, 12, 10) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.04m, Date = new DateTime(2012, 12, 15) });
            incomes.Add(new Income { AccountId = 1, ActualValue = 10.05m, Date = new DateTime(2012, 12, 25) });

            decimal incomesForGivenDate = 0;

            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 11, 01));
            Assert.IsTrue(incomesForGivenDate == 0m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 01));
            Assert.IsTrue(incomesForGivenDate == 0m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 02));
            Assert.IsTrue(incomesForGivenDate == 10.01m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 03));
            Assert.IsTrue(incomesForGivenDate == 10.01m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 05));
            Assert.IsTrue(incomesForGivenDate == 20.03m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 06));
            Assert.IsTrue(incomesForGivenDate == 20.03m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 10));
            Assert.IsTrue(incomesForGivenDate == 30.06m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 11));
            Assert.IsTrue(incomesForGivenDate == 30.06m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 15));
            Assert.IsTrue(incomesForGivenDate == 40.10m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 16));
            Assert.IsTrue(incomesForGivenDate == 40.10m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 25));
            Assert.IsTrue(incomesForGivenDate == 50.15m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2012, 12, 30));
            Assert.IsTrue(incomesForGivenDate == 50.15m);
            incomesForGivenDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(incomes, new DateTime(2013, 01, 01));
            Assert.IsTrue(incomesForGivenDate == 0m);



        }

        [Test]
        public void Can_Sum_Expenses_Whole_Month()
        {
            
        }

    
        [Test]
        public void Can_Sum_Expenses_Up_To_Given_Date_In_Current_Month()
        {
            
        }

        
    }
}