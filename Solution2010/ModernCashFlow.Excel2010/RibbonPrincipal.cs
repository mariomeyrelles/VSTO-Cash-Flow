using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.Forms;
using ModernCashFlow.Globalization.Resources;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010
{
    public partial class RibbonPrincipal
    {
        private void RibbonPrincipal_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            //var formTeste1 = new FormExpense();
            //formTeste1.ShowDialog();
        }


        private void lerSaidas_Click(object sender, RibbonControlEventArgs e)
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Expense>>();
            controller.GetLocalDataAndSyncronizeSession();
        }

        private void btnEscreverSaidas_Click(object sender, RibbonControlEventArgs e)
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Expense>>();
            controller.RefreshAllLocalData();
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            var wks = NinjectContainer.Kernel.Get<ExpenseWorksheet>();
            
        }

        private void btnPendingPayments_Click(object sender, RibbonControlEventArgs e)
        {
            //todo: create a command when more tests are needed
            //var commandManager = NinjectContainer.Kernel.Get<CommandManager>();
            //commandManager.LoadAllTransactions();
            //commandManager.ProcessTodayPayments();
        }

        private void button3_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show(Lang.TesteGlobalizacao);
        }

        private void toggleButton1_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisWorkbook.ThisApplication.CellDragAndDrop = !Globals.ThisWorkbook.ThisApplication.CellDragAndDrop;
        }

        private void button4_Click(object sender, RibbonControlEventArgs e)
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Income>>();
            controller.GetLocalDataAndSyncronizeSession();
        }

        private void button5_Click(object sender, RibbonControlEventArgs e)
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Income>>();
            controller.RefreshAllLocalData();
        }

        private void button6_Click(object sender, RibbonControlEventArgs e)
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Account>>();
            controller.GetLocalDataAndSyncronizeSession();
        }

        private void button7_Click(object sender, RibbonControlEventArgs e)
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Account>>();
            controller.RefreshAllLocalData();
        }

    }
}
