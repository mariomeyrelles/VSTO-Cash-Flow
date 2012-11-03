using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModernCashFlow.Domain.Dtos;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Excel2010.Forms
{
    /// <summary>
    /// This class, even though it's a Windows Forms class, works as a view model for the WPF child control.
    /// </summary>
    public partial class FormPendingExpensesViewModel : Form
    {
        public FormPendingExpensesViewModel(List<EditPendingExpenseDto> todayPayments)
        {
            InitializeComponent();
            this.TodayPayments = todayPayments;
            this.pendingPayments1.ModelData = this;
            this.SaveAndCloseCommand = new RelayCommand(param => this.Save(), param=>this.CanSave());
            this.MarkPaymentAsOkCommand = new RelayCommand(this.MarkPaymentAsOk, param => this.CanMarkPaymentAsOk());
        }

        private bool CanMarkPaymentAsOk()
        {
            return true;
        }

        private void MarkPaymentAsOk(object state)
        {
            var transactionCode = (Guid) state;
            TodayPayments.Single(x => x.Transaction.TransactionCode == transactionCode).IsOk =
                !TodayPayments.Single(x => x.Transaction.TransactionCode == transactionCode).IsOk;
        }

        private bool CanSave()
        {
            //always can save.
            return true;
        }

        private void Save()
        {
            foreach (var payment in TodayPayments)
            {
                payment.Transaction.TransactionStatus = payment.IsOk ? TransactionStatus.OK : TransactionStatus.Pending;
            }


            //ComingPayments.Where(x => x.IsOk).ToList().ForEach(x => x.Transaction.TransactionStatus = TransactionStatus.OK);
            //ComingPayments.Where(x => !x.IsOk).ToList().ForEach(x => x.Transaction.TransactionStatus = TransactionStatus.Scheduled);


            //LatePayments.Where(x => x.IsOk).ToList().ForEach(x => x.Transaction.TransactionStatus = TransactionStatus.OK);
            //LatePayments.Where(x => !x.IsOk).ToList().ForEach(x => x.Transaction.TransactionStatus = TransactionStatus.Pending);

            this.Close();
        }




        public List<EditPendingExpenseDto> TodayPayments { get; set; }

        public List<EditPendingExpenseDto> ComingPayments { get; set; }

        public List<EditPendingExpenseDto> LatePayments { get; set; }


        public RelayCommand SaveAndCloseCommand { get; private set; }
        public RelayCommand MarkPaymentAsOkCommand { get; private set; }

    }
}
