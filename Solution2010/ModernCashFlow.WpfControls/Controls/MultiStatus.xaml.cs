using System.Diagnostics;
using System.Windows.Controls;

namespace ModernCashFlow.WpfControls.Controls
{
	/// <summary>
	/// Interaction logic for MultiStatus.xaml
	/// </summary>
	public partial class MultiStatus : UserControl
	{
		public MultiStatus()
		{
		    this.InitializeComponent();
            //this.DataContextChanged += MultiStatus_DataContextChanged;
           
		}

        void MultiStatus_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue ?? "data context nulo");
        }

      
	}
}