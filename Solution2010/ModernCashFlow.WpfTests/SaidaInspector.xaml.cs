using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModernCashFlow.WpfTests
{
    /// <summary>
    /// Interaction logic for SaidaInspector.xaml
    /// </summary>
    public partial class SaidaInspector : UserControl
    {
        public SaidaInspector()
        {
            InitializeComponent();
        }

        public dynamic ModelData
        {
            set
            {
                this.DataContext = null;
                this.DataContext = value;
            }
            get { return this.DataContext; }
        }
    }
}
