using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Dtos;
using ModernCashFlow.Tools;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Domain.Services
{
    public class ExpenseStatusService
    {
        public IEnumerable<EditPendingExpenseDto> GetTodayPayments(IEnumerable<Expense> allPayments)
        {
            var today = DateTime.Now.Today();

            var todayPayments = from x in allPayments
                                where (x.Date == today)
                                      &&
                                      (x.TransactionStatus == TransactionStatus.Scheduled ||
                                       x.TransactionStatus == TransactionStatus.Pending)
                                select new EditPendingExpenseDto(x);

            return todayPayments;

        }

        public IEnumerable<EditPendingExpenseDto> GetLatePayments(IEnumerable<Expense> allPayments)
        {

            var latePayments = from x in allPayments
                                where x.TransactionStatus == TransactionStatus.Pending && x.Date < DateTime.Now.Today()
                                select new EditPendingExpenseDto(x);

            return latePayments;

        }

        public IEnumerable<EditPendingExpenseDto> GetComingPayments(IEnumerable<Expense> allPayments)
        {

            var nextPayments = from x in allPayments
                                where x.TransactionStatus == TransactionStatus.Scheduled
                                select new EditPendingExpenseDto(x);

            return nextPayments;

        }
    }
}