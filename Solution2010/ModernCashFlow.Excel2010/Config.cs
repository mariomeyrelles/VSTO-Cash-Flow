﻿namespace ModernCashFlow.Excel2010
{
    public partial class Config
    {
        private void Config_Startup(object sender, System.EventArgs e)
        {
            ThisWorkbook.NotifySheetLoaded(this);
        }

        private void Config_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(this.Config_Startup);
            this.Shutdown += new System.EventHandler(this.Config_Shutdown);

        }

        #endregion
        
    }
}
