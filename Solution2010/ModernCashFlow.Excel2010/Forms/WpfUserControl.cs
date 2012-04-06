using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModernCashFlow.Domain.Entities;


namespace ModernCashFlow.Excel2010.Forms
{
    public partial class WpfUserControl : UserControl
    {
        public WpfUserControl()
        {
            InitializeComponent();
        }

        //todo: tirar esse user control
        public dynamic Model
        {
            set
            {
                saidaInspector1.ModelData = value;
            }
        }
    }
}
