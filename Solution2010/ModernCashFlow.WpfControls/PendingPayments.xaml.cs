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
using System.ComponentModel;
using ModernCashFlow.Domain.Dtos;
using Telerik.Windows.Controls;

namespace ModernCashFlow.WpfControls
{
    //todo: atualizar nomes das propriedades pra inglês.

    /// <summary>
    /// Interaction logic for PendingPayments.xaml
    /// </summary>
    public partial class PendingPayments : UserControl
    {
        public PendingPayments()
        {
            InitializeComponent();

           
        }

        public dynamic ModelData
        {
            set
            {
                this.DataContext = value;
                this.gridPendingPayments.ItemsSource = value.LatePayments;
            }
            get
            {
                return this.DataContext;
            }
        }



        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;

            if (!cell.IsEditing)
            {
                // enables editing on single click

                if (!cell.IsFocused)

                    cell.Focus();

                if (!cell.IsSelected)

                    cell.IsSelected = true;

            }
        }

        
    }
}
