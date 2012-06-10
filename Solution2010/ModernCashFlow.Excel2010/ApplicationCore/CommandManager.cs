using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.BaseInterfaces;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.Forms;
using ModernCashFlow.WpfControls;
using Ninject;
using ModernCashFlow.Domain.Dtos;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Responsible to coordinate the interaction between various parts of the program, like panels, ribbons, external services and worksheet data.
    /// </summary>
    public class CommandManager
    {
        private SidePanelWpfHost _sidePanelHost;


        [Inject]       
        public BaseController<Expense> ExpenseController { get; set; }

        [Inject]
        public BaseController<Income> IncomeController { get; set; }

        [Inject]
        public BaseController<Account> AccountController { get; set; }


        //todo: create formal commands
    

        public void UpdateSidePanel(dynamic entity)
        {
            if (_sidePanelHost == null) return;
            _sidePanelHost.Model = entity;
            _sidePanelHost.Refresh();
        }

        public void ShowSplashWindow()
        {
            ProcessTodayPayments();
        }

        public void LoadAccountData()
        {
            AccountController.GetLocalDataAndSyncronizeSession();
        }

        public void LoadAllTransactions()
        {
            ExpenseController.GetLocalDataAndSyncronizeSession();
            IncomeController.GetLocalDataAndSyncronizeSession(); 
        }

        public void ConvertTodayPaymentsToPending()
        {
            var paymentSvc = NinjectContainer.Kernel.Get<ExpenseStatusService>();
            var todayPayments = paymentSvc.GetTodayPayments(ExpenseController.CurrentSessionData).ToList();
            todayPayments.ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Pending);
        }

        public void ProcessTodayPayments()
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


        public void ConfigureSidePanel()
        {
            _sidePanelHost = new SidePanelWpfHost();
            _sidePanelHost.CurrentControl = new SaidaInspector();
            Globals.ThisWorkbook.ActionsPane.Controls.Add(_sidePanelHost);

            //solicitar o refresh do host do wpf sempre que o panel mudar de tamanho ou acontecer algum scroll.
            Globals.ThisWorkbook.ActionsPane.Resize += delegate { _sidePanelHost.Refresh(); };
            Globals.ThisWorkbook.ActionsPane.Scroll += delegate { _sidePanelHost.Refresh(); };
        }


        public void IncludeNewExpenseTransactions()
        {
            
            foreach (var expense in ExpenseController.CurrentSessionData.Where(e => e.IsTransient))
            {
                expense.EditStatus = expense.IsValid ? EditStatus.Complete : EditStatus.Incomplete;
            }
            ExpenseController.RefreshAllLocalData();
        }

        public void IncludeNewIncomeTransactions()
        {
            foreach (var income in IncomeController.CurrentSessionData.Where(i => i.IsTransient))
            {
                income.EditStatus = income.IsValid ? EditStatus.Complete : EditStatus.Incomplete;
            }
            ExpenseController.RefreshAllLocalData();
        }


        public void WriteAllTransactionsToWorsheets()
        {
            ExpenseController.RefreshAllLocalData();
            IncomeController.RefreshAllLocalData();
        }
       
    }
}