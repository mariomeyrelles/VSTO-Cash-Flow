using Microsoft.Office.Tools.Excel;
using ModernCashFlow.Excel2010.ApplicationCore.Factories;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ModernCashFlow.Excel2010
{
    public partial class Expenses
    {
        
        private IExpenseWorksheetFactory _factory;


        
        private void ExpensesStartup(object sender, System.EventArgs e)
        {
            this.tblExpenses.Change += ExpensesChange;
            this.tblExpenses.BeforeRightClick += ExpensesBeforeRightClick;
            this.tblExpenses.SelectionChange += ExpensesSelectionChange;
          
            
            ThisWorkbook.NotifySheetLoaded(this);

            //todo: review DI process for the factory itself.
            _factory = NinjectContainer.Kernel.Get<IExpenseWorksheetFactory>();
        }

        void ExpensesSelectionChange(Excel.Range target)
        {
            var eventHandlers = _factory.CreateEventHandlers();
            eventHandlers.OnSelectionChange(target);
        }

        void ExpensesBeforeRightClick(Excel.Range target, ref bool cancel)
        {
            try
            {
                Application.EnableEvents = false;

                var popup = _factory.CreateContextMenu();
                popup.ShowContextMenu(target, ref cancel);
            }
            finally
            {
                Application.EnableEvents = true;
            }
        }

        private void ExpensesChange(Excel.Range target, ListRanges changedRanges)
        {
            try
            {
                Application.EnableEvents = false;

                var eventHandlers = _factory.CreateEventHandlers();
                eventHandlers.OnChange(target, changedRanges);
            }
            finally
            {
                Application.EnableEvents = true;
            }
        }

        private void Expenses_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += ExpensesStartup;
            this.Shutdown += Expenses_Shutdown;
            
        }

        
        #endregion

      
    }
}
