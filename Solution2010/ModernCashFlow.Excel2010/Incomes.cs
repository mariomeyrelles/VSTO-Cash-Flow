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
            this.tblEntradas.Change += (this.tblEntradas_Change);
            this.tblEntradas.BeforeRightClick += (tblEntradas_BeforeRightClick);
            this.tblEntradas.SelectionChange += (tblEntradas_SelectionChange);


            var wksHelper = NinjectContainer.Kernel.Get<IncomeWorksheet>();
            wksHelper.ReadColumnPositions();
            wksHelper.ConfigureValidationLists();
            ThisWorkbook.NotifySheetLoaded(this);
        }


        void tblEntradas_SelectionChange(Range target)
        {
            var eventHandlers = NinjectContainer.Kernel.Get<IncomeWorksheet.Events>();
            eventHandlers.OnSelectionChange(target);

        }

        void tblEntradas_BeforeRightClick(Range target, ref bool cancel)
        {
            Application.EnableEvents = false;

            var popup = NinjectContainer.Kernel.Get<IncomeWorksheet.ContextMenus>();
            popup.ShowContextMenu(target, ref cancel);

            Application.EnableEvents = true;
        }


        private void tblEntradas_Change(Range target, ListRanges changedRanges)
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
