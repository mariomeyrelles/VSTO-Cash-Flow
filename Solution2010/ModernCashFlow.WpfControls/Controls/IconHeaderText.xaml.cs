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


       
        #region MainContent Dependency Property

        public static readonly DependencyProperty MainContentProperty = DependencyProperty.Register("MainContent", typeof(object), typeof(IconHeaderText),
                                       new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, ChangeMainContent));

        private static void ChangeMainContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconHeaderText).mainContent.Content = (e.NewValue);
        }
        
        public object MainContent
        {
            get
            {
                return GetValue(MainContentProperty);
            }
            set
            {
                SetValue(MainContentProperty, value);
            }
        }

        #endregion

        #region Header Text Dependency Property

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(IconHeaderText),
                                        new FrameworkPropertyMetadata("-", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChangeHeaderText));


        private static void ChangeHeaderText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconHeaderText).headerContent.Text = e.NewValue.ToString();
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

        #region Content Text Dependency Property

        public static readonly DependencyProperty ContentTextProperty = DependencyProperty.Register("ContentText", typeof(string), typeof(IconHeaderText),
                                        new FrameworkPropertyMetadata("-", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChangeContentText));


        private static void ChangeContentText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconHeaderText).textContent.Text = e.NewValue.ToString();
        }


        public string ContentText
        {
            get
            {
                return (string)GetValue(ContentTextProperty);
            }
            set
            {
                SetValue(ContentTextProperty, value);
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
