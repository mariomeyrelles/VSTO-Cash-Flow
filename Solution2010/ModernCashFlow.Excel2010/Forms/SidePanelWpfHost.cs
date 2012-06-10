using System;
using System.Windows;
using System.Windows.Forms;
using Wpf = System.Windows;


namespace ModernCashFlow.Excel2010.Forms
{
    public partial class SidePanelWpfHost : UserControl
    {
        public SidePanelWpfHost()
        {
            InitializeComponent();
        }

        public dynamic Model
        {
            get { return CurrentControl.DataContext; }
            set
            {
                if (this.CurrentControl != null)
                {
                    CurrentControl.DataContext = value;
                }
                else
                    throw new InvalidOperationException("Can't assign a model to an empty child control.");
            }
        }
        
        public Wpf.Controls.UserControl CurrentControl
        {
            get { return this.elementHost1.Child as Wpf.Controls.UserControl; }
            set { this.elementHost1.Child = value; }
        }

    }
}
