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
        public FormPendingExpensesViewModel()
        {
            InitializeComponent();
            this.pendingPayments1.ModelData = this;
            this.SaveAndCloseCommand = new RelayCommand(param => this.Save(), param=>this.CanSave());
        }

        private bool CanSave()
        {
            //always can save.
            return true;
        }

        private void Save()
        {
            //atualizar os valores para ok.
            TodayPayments.Where(x => x.IsPaid).ToList().ForEach(x => x.Expense.TransactionStatus = TransactionStatus.OK);
            TodayPayments.Where(x => !x.IsPaid).ToList().ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Pending);

            ComingPayments.Where(x => x.IsPaid).ToList().ForEach(x => x.Expense.TransactionStatus = TransactionStatus.OK);
            ComingPayments.Where(x => !x.IsPaid).ToList().ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Scheduled);


            LatePayments.Where(x => x.IsPaid).ToList().ForEach(x => x.Expense.TransactionStatus = TransactionStatus.OK);
            LatePayments.Where(x => !x.IsPaid).ToList().ForEach(x => x.Expense.TransactionStatus = TransactionStatus.Pending);

            this.Close();
        }



        public List<EditPendingExpenseDto> TodayPayments { get; set; }

        public List<EditPendingExpenseDto> ComingPayments { get; set; }

        public List<EditPendingExpenseDto> LatePayments { get; set; }


        public RelayCommand SaveAndCloseCommand { get; private set; }

    }
}
