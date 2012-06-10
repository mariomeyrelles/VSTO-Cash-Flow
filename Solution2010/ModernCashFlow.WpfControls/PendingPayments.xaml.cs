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

            if (IsInDesignModeStatic)
            {
                var valoresTeste = new List<EditPendingExpenseDto>();
                valoresTeste.Add(new EditPendingExpenseDto(new Domain.Entities.Expense() { ExpectedValue = 10, Date = new DateTime(2012, 02, 01) }));
                valoresTeste.Add(new EditPendingExpenseDto(new Domain.Entities.Expense() { ExpectedValue = 20, Date = new DateTime(2012, 02, 02) }));
                valoresTeste.Add(new EditPendingExpenseDto(new Domain.Entities.Expense() { ExpectedValue = 30, Date = new DateTime(2012, 02, 03) }));

                this.gridTodayPayments.ItemsSource = valoresTeste;
            }


        }

        public dynamic ModelData
        {
            set
            {
                this.DataContext = value;
            }
            get
            {
                return this.DataContext;
            }
        }


        private static bool? _isInDesignMode;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend
        /// or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
            _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                        = (bool)DependencyPropertyDescriptor
                        .FromProperty(prop, typeof(FrameworkElement))
                        .Metadata.DefaultValue;
#endif
                }

                return _isInDesignMode.Value;
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
