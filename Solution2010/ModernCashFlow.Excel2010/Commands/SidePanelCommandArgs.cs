using System.Windows.Controls;

namespace ModernCashFlow.Excel2010.Commands
{
    public class SidePanelCommandArgs : CommandArgs
    {
        public UserControl WpfControl { get; set; }
        public object Model { get; set; }
    }
}