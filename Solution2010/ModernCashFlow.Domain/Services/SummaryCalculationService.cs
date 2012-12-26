using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Services
{
    public class SummaryCalculationService
    {
        public decimal CalculateBalanceForCurrentMonth(IEnumerable<BaseTransaction> transactions)
        {
            
            var today = SystemTime.Now();

            var sumOfIncomes = transactions.Where(x => x.Date != null && x.Date.Value.Month == today.Month).Sum(x => x.Value);

            return sumOfIncomes;

        }
       
        public decimal CalculateExpensesForCurrentMonthUpToGivenDate(IEnumerable<BaseTransaction> transactions, DateTime now)
        {
            var expenses = transactions.OfType<Expense>();
          
            var sumOfExpenses = expenses.Where(x => x.Date != null && (x.Date.Value.Month == now.Month && x.Date <= now)).Sum(x => x.Value);

            return sumOfExpenses;
        }

        public decimal CalculateIncomesForCurrentMonthUpToGivenDate(IEnumerable<BaseTransaction> transactions, DateTime now)
        {
            var incomes = transactions.OfType<Income>();
          
            var sumOfIncomes = incomes.Where(x => x.Date != null && (x.Date.Value.Month == now.Month && x.Date <= now)).Sum(x => x.Value);

            return sumOfIncomes;
        }

        public List<AccountSummary> CalculateAccountSummary(IEnumerable<Account> accounts, IEnumerable<BaseTransaction> transactions )
        {
            var baseTransactions = transactions as List<BaseTransaction> ?? transactions.ToList();
            var incomes = baseTransactions.OfType<Income>().ToList();
            var expenses = baseTransactions.OfType<Expense>().ToList();
            
            var summaries = new List<AccountSummary>();
            var svc = new BalanceCalculationService();
            var today = SystemTime.Now();
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = new DateTime(today.Year, today.Month,DateTime.DaysInMonth(today.Year,today.Month));

            //calculate balance for all acccounts today
            var args1 = new CalculationArgs(incomes, expenses) { StartingDate = firstDayOfMonth , EndingDate = today};
            var balanceToday =svc.CalculateBalance(args1);

            //calculate the balance in the end of the month to build the previsions column in the grid
            var args2 = new CalculationArgs(incomes, expenses) { StartingDate = firstDayOfMonth , EndingDate = lastDayOfMonth};
            var balanceEOM =svc.CalculateBalance(args2);
            
            foreach (var account in accounts)
            {
                var accountSummary = new AccountSummary();
                accountSummary.AccountId = account.Id;
                accountSummary.AccountName = account.Name;
                accountSummary.CurrentBalance = balanceToday.ForAccountId(account.Id);
                accountSummary.EndOfMonthBalance = balanceEOM.ForAccountId(account.Id);
                summaries.Add(accountSummary);
            }
            
            return summaries;

        }
        
    }

    public class AccountSummary
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal EndOfMonthBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}