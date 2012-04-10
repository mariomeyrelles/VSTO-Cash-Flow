using Microsoft.Office.Tools.Excel;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ModernCashFlow.Excel2010
{
    public partial class Expenses
    {
        private void Expenses_Startup(object sender, System.EventArgs e)
        {

            this.tblSaidas.Change += (this.tblSaidas_Change);
            this.tblSaidas.BeforeRightClick += (tblSaidas_BeforeRightClick);
            this.tblSaidas.SelectionChange += (tblSaidas_SelectionChange);


            var wksHelper = NinjectContainer.Kernel.Get<ExpenseWorksheet>();
            wksHelper.ReadColumnPositions();
            wksHelper.ConfigureValidationLists();

            ThisWorkbook.NotifySheetLoaded(this);
        }

        void tblSaidas_SelectionChange(Excel.Range target)
        {
            var eventHandlers = NinjectContainer.Kernel.Get<ExpenseWorksheet.Events>();
            eventHandlers.OnSelectionChange(target);
        }

        void tblSaidas_BeforeRightClick(Excel.Range target, ref bool cancel)
        {
            Application.EnableEvents = false;

            var popup = NinjectContainer.Kernel.Get<ExpenseWorksheet.ContextMenus>();
            popup.ShowContextMenu(target, ref cancel);

            Application.EnableEvents = true;
        }

        private void tblSaidas_Change(Excel.Range target, ListRanges changedRanges)
        {
            //todo: analisar se é preciso colocar try catch para manter os eventos da app ativos mesmo em caso de erro.
            Application.EnableEvents = false;

            var eventHandlers = NinjectContainer.Kernel.Get<ExpenseWorksheet.Events>();
            eventHandlers.OnChange(target, changedRanges);

            Application.EnableEvents = true;
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
            this.Startup += new System.EventHandler(Expenses_Startup);
            this.Shutdown += new System.EventHandler(Expenses_Shutdown);
        }

        #endregion

    }
}
