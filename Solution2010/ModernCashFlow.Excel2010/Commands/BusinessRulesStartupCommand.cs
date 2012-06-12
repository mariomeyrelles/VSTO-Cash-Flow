using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Dtos;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.Forms;
using Ninject;

namespace ModernCashFlow.Excel2010.Commands
{
    public class BusinessRulesStartupCommand
    {


        [Inject]
        public BaseController<Expense> ExpenseController { get; set; }

        [Inject]
        public BaseController<Income> IncomeController { get; set; }

        [Inject]
        public BaseController<Account> AccountController { get; set; }

        public void Execute(CommandArgs args)
        {
            LoadAllTransactions();
            //ConvertTodayPaymentsToPending();
            //WriteAllTransactionsToWorsheets();
            //ShowSplashWindow();
           

        }

        private void LoadAllTransactions()
        {
            ExpenseController.GetLocalDataAndSyncronizeSession();
            IncomeController.GetLocalDataAndSyncronizeSession();
        }



        private void ConvertTodayPaymentsToPending()
        {
            var paymentSvc = NinjectContainer.Kernel.Get<ExpenseStatusService>();
            var todayPayments = paymentSvc.GetTodayPayments(ExpenseController.CurrentSessionData).ToList();
            todayPayments.ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Pending);
        }

        private void WriteAllTransactionsToWorsheets()
        {
            ExpenseController.RefreshAllLocalData();
            IncomeController.RefreshAllLocalData();
        }

        private void ShowSplashWindow()
        {
            ProcessTodayPayments();
        }

        private void ProcessTodayPayments()
        {
            var paymentSvc = NinjectContainer.Kernel.Get<ExpenseStatusService>();
            var todayPayments = paymentSvc.GetTodayPayments(ExpenseController.CurrentSessionData).ToList();
            var comingPayments = paymentSvc.GetComingPayments(ExpenseController.CurrentSessionData).ToList();
            var latePayments = paymentSvc.GetLatePayments(ExpenseController.CurrentSessionData).ToList();

            var form = new FormPendingExpensesViewModel { TodayPayments = todayPayments, ComingPayments = comingPayments, LatePayments = latePayments };
            form.ShowDialog();

            //when the form is closed, read the modified data and notify the worksheet.
            var processedPayments = new List<Expense>();
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.TodayPayments, w => w.IsPaid == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.LatePayments, w => w.IsPaid == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.ComingPayments, w => w.IsPaid == true));

            ExpenseController.AcceptDataCollection(processedPayments, true);

        }
    }
}