using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010
{
    public partial class Incomes
    {
        private void Incomes_Startup(object sender, System.EventArgs e)
        {
            this.tblIncomes.Change += (this.tblIncomes_Change);
            this.tblIncomes.BeforeRightClick += (tblIncomes_BeforeRightClick);
            this.tblIncomes.SelectionChange += (tblIncomes_SelectionChange);

            ThisWorkbook.NotifySheetLoaded(this);
        }


        void tblIncomes_SelectionChange(Range target)
        {
            var eventHandlers = NinjectContainer.Kernel.Get<IncomeWorksheet.Events>();
            eventHandlers.OnSelectionChange(target);

        }

        void tblIncomes_BeforeRightClick(Range target, ref bool cancel)
        {
            Application.EnableEvents = false;

            var popup = NinjectContainer.Kernel.Get<IncomeWorksheet.ContextMenus>();
            popup.ShowContextMenu(target, ref cancel);

            Application.EnableEvents = true;
        }


        private void tblIncomes_Change(Range target, ListRanges changedRanges)
        {
            //todo: analisar se é preciso colocar try catch para manter os eventos da app ativos mesmo em caso de erro.
            Application.EnableEvents = false;

            var eventHandlers = NinjectContainer.Kernel.Get<IncomeWorksheet.Events>();
            eventHandlers.OnChange(target, changedRanges);

            Application.EnableEvents = true;
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
