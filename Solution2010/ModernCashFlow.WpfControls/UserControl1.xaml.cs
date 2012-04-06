using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.WpfControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            //ResourceDictionary r = new ResourceDictionary();
            ////<ResourceDictionary Source="ReuxablesLegacy;component/edge.xaml" />
            //r.Source = new Uri("ReuxablesLegacy;component/edge.xaml",UriKind.RelativeOrAbsolute);
            //this.Resources.Add(r,r);
        }

        public dynamic ModelData
        {
            set 
            { 
                this.DataContext = value;
            }
            get { return this.DataContext; }
        }

        public event EventHandler Save;
        public event EventHandler Next;
        public event EventHandler Previous;
        
        private void OnNext(EventArgs e)
        {
            var handler = Next;
            if (handler != null) handler(this, e);
        }
        
        private void OnPrevious(EventArgs e)
        {
            var handler = Previous;
            if (handler != null) handler(this, e);
        }

        private void OnSave(EventArgs e)
        {
            var handler = Save;
            if (handler != null) handler(this,e);
        }



        private void BtnNovoClick(object sender, RoutedEventArgs e)
        {
            //todo: criar processo para lançar nova saída.
        }

        private void BtnSalvarClick(object sender, RoutedEventArgs e)
        {
            OnSave(e);
        }

        private void BtnPrevClick(object sender, RoutedEventArgs e)
        {
            OnPrevious(e);
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            OnNext(e);
        }
    }
}
