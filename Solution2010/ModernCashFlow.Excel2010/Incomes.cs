using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using ModernCashFlow.Excel2010.ApplicationCore.Factories;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010
{
    public partial class Incomes
    {

        private IIncomeWorksheetFactory _factory;

        private void Incomes_Startup(object sender, System.EventArgs e)
        {
            this.tblIncomes.Change += (this.tblIncomes_Change);
            this.tblIncomes.BeforeRightClick += (tblIncomes_BeforeRightClick);
            this.tblIncomes.SelectionChange += (tblIncomes_SelectionChange);

            ThisWorkbook.NotifySheetLoaded(this);

            //todo: review DI process for the factory itself.
            _factory = NinjectContainer.Kernel.Get<IIncomeWorksheetFactory>();
        }


        void tblIncomes_SelectionChange(Range target)
        {
            var eventHandlers = _factory.CreateEventHandlers();
            eventHandlers.OnSelectionChange(target);
        }

        void tblIncomes_BeforeRightClick(Range target, ref bool cancel)
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


        private void tblIncomes_Change(Range target, ListRanges changedRanges)
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
        private void Incomes_Shutdown(object sender, System.EventArgs e)
        {
        }


        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Incomes_Startup);
            this.Shutdown += new System.EventHandler(Incomes_Shutdown);
        }

        #endregion

    }
}
