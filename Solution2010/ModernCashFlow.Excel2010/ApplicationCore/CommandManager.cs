using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.BaseInterfaces;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.Forms;
using Ninject;
using ModernCashFlow.Domain.Dtos;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Responsible to coordinate the interaction between various parts of the program, like panels, ribbons, external services and worksheet data.
    /// </summary>
    public class CommandManager
    {
        private WpfUserControl _saidaInspector;
        private BaseController<Expense> _paymentController;
        private BaseController<Income> _entradaController;


        //todo: create formal commands

        public void UpdateSidePanel(dynamic entity)
        {
            if (_saidaInspector == null) return;
            _saidaInspector.Model = entity;
            _saidaInspector.Refresh();
            //var form = new FormSaida() { Model = entity };
            //form.Show();
           

        }

        public void ShowSplashWindow()
        {
            ProcessTodayPayments();
        }

        public void LoadAllPayments()
        {
            //todo: tirar esta lógica deste comando.
            _paymentController = _paymentController ?? NinjectContainer.Kernel.Get<BaseController<Expense>>();
            _entradaController = _entradaController ?? NinjectContainer.Kernel.Get<BaseController<Income>>();
            _paymentController.GetLocalDataAndSyncronizeSession();
            _entradaController.GetLocalDataAndSyncronizeSession(); 
        }

        public void ConvertTodayPaymentsToPending()
        {
            _paymentController = _paymentController ?? NinjectContainer.Kernel.Get<BaseController<Expense>>();
            var paymentSvc = NinjectContainer.Kernel.Get<ExpenseStatusService>();

            var todayPayments = paymentSvc.GetTodayPayments(_paymentController.CurrentSessionData).ToList();

            todayPayments.ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Pending);
            _paymentController.RefreshAllLocalData();

        }

        public void ProcessTodayPayments()
        {
            var paymentSvc = NinjectContainer.Kernel.Get<ExpenseStatusService>();
            var todayPayments = paymentSvc.GetTodayPayments(_paymentController.CurrentSessionData).ToList();
            var comingPayments = paymentSvc.GetComingPayments(_paymentController.CurrentSessionData).ToList();
            var latePayments = paymentSvc.GetLatePayments(_paymentController.CurrentSessionData).ToList();

            var form = new FormPendingExpensesViewModel { TodayPayments = todayPayments, ComingPayments = comingPayments, LatePayments = latePayments };
            form.ShowDialog();

            //when the form is closed, read the modified data and notify the worksheet.
            var processedPayments = new List<Expense>();
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.TodayPayments, w => w.IsPaid == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.LatePayments, w => w.IsPaid == true));
            processedPayments.AddRange(EditPendingExpenseDto.ToList(form.ComingPayments, w => w.IsPaid == true));

            _paymentController.AcceptDataCollection(processedPayments, true);

        }



        public void ConfigureSidePanel()
        {
            _saidaInspector = new WpfUserControl();
            Globals.ThisWorkbook.ActionsPane.Controls.Add(_saidaInspector);
            //solicitar o refresh do host do wpf sempre que o panel mudar de tamanho ou acontecer algum scroll.
            Globals.ThisWorkbook.ActionsPane.Resize += delegate { _saidaInspector.Refresh(); };
            Globals.ThisWorkbook.ActionsPane.Scroll += delegate { _saidaInspector.Refresh(); };
        }


        public void IncluirSaidas()
        {
            
            foreach (var saida in _paymentController.CurrentSessionData.Where(saida => saida.IsTransient))
            {
                saida.EditStatus = saida.IsValid ? EditStatus.Complete : EditStatus.Incomplete;
            }
            _paymentController.RefreshAllLocalData();

        }
    }
}