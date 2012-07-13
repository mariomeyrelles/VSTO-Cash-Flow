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

namespace ModernCashFlow.WpfControls.Controls
{
    /// <summary>
    /// Interaction logic for IconHeaderText.xaml
    /// </summary>
    public partial class IconHeaderText : UserControl
    {
        public IconHeaderText()
        {
            InitializeComponent();
        }


        #region Text Dependency Property

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IconHeaderText),
                                        new FrameworkPropertyMetadata("-", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChangeText));

        private static void ChangeText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconHeaderText).textContent.Text = e.NewValue.ToString();
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion



        #region Header Text Dependency Property

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(IconHeaderText),
                                        new FrameworkPropertyMetadata("-", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChangeHeaderText));


        private static void ChangeHeaderText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconHeaderText).textHeader.Text = e.NewValue.ToString();
        }


        public string HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }

        #endregion



        #region Icon Dependency Property


        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(IconHeaderText), new FrameworkPropertyMetadata(null, ChangeIcon));

        private static void ChangeIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconHeaderText).iconControl.Content = e.NewValue;
        }


        public object Icon
        {
            get
            {
                return GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        #endregion

    }
}
