using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernCashFlow.WpfControls.Resources.XamlIcons
{
	/// <summary>
	/// Interaction logic for NotOkayIcon.xaml
	/// </summary>
	public partial class NotOkayIcon : UserControl
	{
		public NotOkayIcon()
		{
			this.InitializeComponent();
		}

        public static readonly DependencyProperty BackgroundBrushProperty =
           DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(NotOkayIcon), new FrameworkPropertyMetadata(null, ChangeBackgroundBrush));

        private static void ChangeBackgroundBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NotOkayIcon)d).Rect.Fill = (Brush)e.NewValue;
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