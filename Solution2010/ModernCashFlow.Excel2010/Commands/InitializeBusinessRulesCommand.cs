using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.Dtos;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.Forms;
using ModernCashFlow.WpfControls;
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
            ConvertTodayPaymentsToPending();
            //WriteAllTransactionsToWorsheets();
            ShowSplashWindow();
        }

        private void LoadAllTransactions()
        {
            _expenseController.GetLocalDataAndSyncronizeSession();
            _incomeController.GetLocalDataAndSyncronizeSession();
        }


        private void ConvertTodayPaymentsToPending()
        {
            var todaysExpenses = _paymentSvc.GetTodayPayments(_expenseController.CurrentSessionData).ToList();
            todaysExpenses.ForEach(x => x.Transaction.TransactionStatus = TransactionStatus.Pending);

            var todaysIncomes = _paymentSvc.GetTodayPayments(_incomeController.CurrentSessionData).ToList();
            todaysIncomes.ForEach(x => x.Transaction.TransactionStatus = TransactionStatus.Pending);

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
            var nextPayments = _paymentSvc.GetComingPayments(_expenseController.CurrentSessionData).ToList();
            var latePayments = _paymentSvc.GetLatePayments(_expenseController.CurrentSessionData).ToList();

            var form = new FormPendingExpensesViewModel(todayPayments, nextPayments, latePayments);
            form.ShowDialog();

            //when the form is closed, read the modified data and notify the worksheet.
            var processedPayments = new List<BaseTransaction>();
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.TodayPayments, w => w.IsOk == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.LatePayments, w => w.IsOk == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.NextPayments, w => w.IsOk == true));

            var processedExpenses = processedPayments.OfType<Expense>();
            var processedIncomes = processedPayments.OfType<Income>();
            _expenseController.AcceptDataCollection(processedExpenses, true);
            _incomeController.AcceptDataCollection(processedIncomes, true);

            CommandHandler.Run<ConfigureSidePanelCommand>(new SidePanelCommandArgs { WpfControl = new MainSidePanel() });


        }
    }
}