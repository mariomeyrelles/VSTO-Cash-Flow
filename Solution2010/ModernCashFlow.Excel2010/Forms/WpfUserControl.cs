using System.Windows.Forms;


namespace ModernCashFlow.Excel2010.Forms
{
    public partial class WpfUserControl : UserControl
    {
        public WpfUserControl()
        {
            InitializeComponent();
        }

        public dynamic Model
        {
            set
            {
                saidaInspector1.ModelData = value;
            }
        }
    }
}
