namespace ModernCashFlow.Excel2010
{
    public partial class Sheet3
    {
        private void Sheet3_Startup(object sender, System.EventArgs e)
        {
            ThisWorkbook.NotifySheetLoaded(this);
        }

        private void Sheet3_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet3_Startup);
            this.Shutdown += new System.EventHandler(Sheet3_Shutdown);
        }

        #endregion

    }
}
