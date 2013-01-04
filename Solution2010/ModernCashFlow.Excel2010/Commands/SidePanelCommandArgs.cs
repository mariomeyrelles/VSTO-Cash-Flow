using System.Collections.Generic;
using System.Windows.Controls;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Excel2010.Commands
{
    public class SidePanelCommandArgs : CommandArgs
    {
        public UserControl WpfControl { get; set; }
        public object Model { get; set; }
        public IEnumerable<BaseTransaction> Transactions { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
    }
}