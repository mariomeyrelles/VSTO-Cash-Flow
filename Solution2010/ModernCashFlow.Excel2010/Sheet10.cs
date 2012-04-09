﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ModernCashFlow.Excel2010
{
    public partial class Sheet10
    {
        private void Sheet10_Startup(object sender, System.EventArgs e)
        {
            ThisWorkbook.NotifySheetLoaded(this);
        }

        private void Sheet10_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet10_Startup);
            this.Shutdown += new System.EventHandler(Sheet10_Shutdown);
        }

        #endregion

    }
}
