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
    public class InitializeBusinessRulesCommand : ICommand
    {

        private readonly BaseController<Expense> _expenseController;
        private readonly BaseController<Income> _incomeController;
        private readonly BaseController<Account> _accountController;
        private readonly ExpenseStatusService _paymentSvc;

        public InitializeBusinessRulesCommand(BaseController<Expense> expenseController,
            BaseController<Income> incomeController, BaseController<Account> accountController,
            ExpenseStatusService paymentStatusService)
        {
            _expenseController = expenseController;
            _incomeController = incomeController;
            _accountController = accountController;
            _paymentSvc = paymentStatusService;
        }

        public void Execute(CommandArgs args)
        {
            LoadAllTransactions();
            //ConvertTodayPaymentsToPending();
            //WriteAllTransactionsToWorsheets();
            //ShowSplashWindow();
        }

        private void LoadAllTransactions()
        {
            _expenseController.GetLocalDataAndSyncronizeSession();
            _incomeController.GetLocalDataAndSyncronizeSession();
        }


        private void ConvertTodayPaymentsToPending()
        {
            var todayPayments = _paymentSvc.GetTodayPayments(_expenseController.CurrentSessionData).ToList();
            todayPayments.ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Pending);
        }

        private void WriteAllTransactionsToWorsheets()
        {
            _expenseController.RefreshAllLocalData();
            _incomeController.RefreshAllLocalData();
        }

        private void ShowSplashWindow()
        {
            ProcessTodayPayments();
        }

        private void ProcessTodayPayments()
        {

            var todayPayments = _paymentSvc.GetTodayPayments(_expenseController.CurrentSessionData).ToList();
            var comingPayments = _paymentSvc.GetComingPayments(_expenseController.CurrentSessionData).ToList();
            var latePayments = _paymentSvc.GetLatePayments(_expenseController.CurrentSessionData).ToList();

            var form = new FormPendingExpensesViewModel { TodayPayments = todayPayments, ComingPayments = comingPayments, LatePayments = latePayments };
            form.ShowDialog();

            //when the form is closed, read the modified data and notify the worksheet.
            var processedPayments = new List<Expense>();
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.TodayPayments, w => w.IsPaid == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.LatePayments, w => w.IsPaid == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.ComingPayments, w => w.IsPaid == true));

            _expenseController.AcceptDataCollection(processedPayments, true);

        }
    }
}