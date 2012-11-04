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
        public FormPendingExpensesViewModel(List<EditPendingExpenseDto> todayPayments, 
                                            List<EditPendingExpenseDto> nextPayments,
                                            List<EditPendingExpenseDto> latePayments)
        {
            InitializeComponent();
            
            TodayPayments = todayPayments;
            NextPayments = nextPayments;
            LatePayments = latePayments;

            this.pendingPayments1.ModelData = this;
            
            this.SaveAndCloseCommand = new RelayCommand(param => this.Save(), param => this.CanSave());
            this.MarkPaymentAsOkCommand = new RelayCommand(this.MarkPaymentAsOk, param => this.CanMarkPaymentAsOk());
        }

        private bool CanMarkPaymentAsOk()
        {
            return true;
        }

        private void MarkPaymentAsOk(object state)
        {
            var transactionCode = (Guid) state;

            foreach (var payment in TodayPayments.Where(x => x.Transaction.TransactionCode == transactionCode))
                payment.IsOk = !payment.IsOk;

            foreach (var payment in NextPayments.Where(x => x.Transaction.TransactionCode == transactionCode))
                payment.IsOk = !payment.IsOk;

            foreach (var payment in LatePayments.Where(x => x.Transaction.TransactionCode == transactionCode))
                payment.IsOk = !payment.IsOk;
        }

        private bool CanSave()
        {
            //always can save.
            return true;
        }

        private void Save()
        {
            foreach (var payment in TodayPayments)
                payment.Transaction.TransactionStatus = payment.IsOk ? TransactionStatus.OK : TransactionStatus.Pending;

            foreach (var payment in NextPayments)
                payment.Transaction.TransactionStatus = payment.IsOk ? TransactionStatus.OK : TransactionStatus.Pending;

            foreach (var payment in LatePayments)
                payment.Transaction.TransactionStatus = payment.IsOk ? TransactionStatus.OK : TransactionStatus.Pending;

            this.Close();
        }




        public List<EditPendingExpenseDto> TodayPayments { get; private set; }

        public List<EditPendingExpenseDto> NextPayments { get; private set; }

        public List<EditPendingExpenseDto> LatePayments { get; private set; }


        public RelayCommand SaveAndCloseCommand { get; private set; }
        public RelayCommand MarkPaymentAsOkCommand { get; private set; }

    }
}
