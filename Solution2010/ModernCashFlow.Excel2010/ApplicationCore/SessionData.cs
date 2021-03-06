﻿using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Useful data for the current session of the application.
    /// </summary>
    public class CurrentSession
    {
        /// <summary>
        /// Returns all the transactions that are currently in memory. May not bring archived transactions.
        /// </summary>
        public static IEnumerable<BaseTransaction> AllTransactions
        {
            get
            {
                var transactions = new List<BaseTransaction>();
                transactions.AddRange(SessionDataSingleton<Expense>.Instance);
                transactions.AddRange(SessionDataSingleton<Income>.Instance);
                
                return transactions;
            }
        }

        /// <summary>
        /// Returns all the transactions that are currently in memory. May not bring archived transactions.
        /// </summary>
        public static IEnumerable<BaseTransaction> ValidTransactions
        {
            get
            {
                var transactions = new List<BaseTransaction>();
                transactions.AddRange(SessionDataSingleton<Expense>.Instance.Where(x=>x.CanBeUsedInCashFlow));
                transactions.AddRange(SessionDataSingleton<Income>.Instance.Where(x => x.CanBeUsedInCashFlow));

                return transactions;
            }
        }

        public static IEnumerable<Account> Accounts
        {
            get
            {
                var accounts = new List<Account>();
                accounts.AddRange(SessionDataSingleton<Account>.Instance);
                return accounts;
            }

        }
    }
}