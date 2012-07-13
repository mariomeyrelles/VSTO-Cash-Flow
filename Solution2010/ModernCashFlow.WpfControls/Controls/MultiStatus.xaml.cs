using System.Windows;
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
		}
	}

    public class MultiStatusDataTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null)
            {
               if (item.ToString() == "okay")
                    return element.FindResource("okay") as DataTemplate;

               if (item.ToString() == "notOkay")
                   return element.FindResource("okay") as DataTemplate;
               
            }

            return null;
        }
    }
}