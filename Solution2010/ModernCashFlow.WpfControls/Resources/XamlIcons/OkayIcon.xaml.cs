using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernCashFlow.WpfControls.Resources.XamlIcons
{
    /// <summary>
	/// Interaction logic for OkayIcon.xaml
	/// </summary>
	public partial class OkayIcon : UserControl
	{
		public OkayIcon()
		{
			this.InitializeComponent();
		}


        public static readonly DependencyProperty BackgroundBrushProperty =
           DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(OkayIcon), new FrameworkPropertyMetadata(null, ChangeBackgroundBrush));

        private static void ChangeBackgroundBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           ((OkayIcon) d).Rect.Fill = (Brush) e.NewValue;
        }


        public object BackgroundBrush
        {
            get
            {
                return GetValue(BackgroundBrushProperty);
            }
            set
            {
                SetValue(BackgroundBrushProperty, value);
            }
        }
	}
}