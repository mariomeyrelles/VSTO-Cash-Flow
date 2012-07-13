using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernCashFlow.WpfControls.Controls
{
    /// <summary>
    /// Interaction logic for MoneyValueControl.xaml
    /// </summary>
    public partial class IconText : UserControl
    {
        public IconText()
        {
            this.InitializeComponent();
        }



        #region Text Dependency Property
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IconText),
                                        new FrameworkPropertyMetadata("-",FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChangeText));

        private static void ChangeText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconText).textContent.Text = e.NewValue.ToString();
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



        //#region Header Text Dependency Property

        //public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(IconTextUserControl),
        //                                new FrameworkPropertyMetadata("-", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChangeHeaderText));

        
        //private static void ChangeHeaderText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as IconTextUserControl).textHeader.Text = e.NewValue.ToString();
        //}

       
        //public string HeaderText
        //{
        //    get
        //    {
        //        return (string)GetValue(HeaderTextProperty);
        //    }
        //    set
        //    {
        //        SetValue(HeaderTextProperty, value);
        //    }
        //}

        //#endregion



        #region Icon Dependency Property


        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(IconText), new FrameworkPropertyMetadata(null, ChangeIcon));

        private static void ChangeIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IconText).iconControl.Content = e.NewValue;
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