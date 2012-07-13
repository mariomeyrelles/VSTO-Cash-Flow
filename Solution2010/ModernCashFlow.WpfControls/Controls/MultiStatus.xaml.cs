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
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (item == null)
            {
                return null;
            }
            
            var element = container as FrameworkElement;
            var value = (item as dynamic).StatusName;

            if (element != null && value != null)
            {
               if (value == "okay")
                    return element.FindResource("okay") as DataTemplate;

               if (value == "notOkay")
                   return element.FindResource("notOkay") as DataTemplate;
               
            }

            return null;
        }
    }
}