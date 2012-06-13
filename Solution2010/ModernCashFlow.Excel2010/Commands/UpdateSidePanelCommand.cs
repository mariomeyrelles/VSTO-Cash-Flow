using System;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.WpfControls;

namespace ModernCashFlow.Excel2010.Commands
{
    public class UpdateSidePanelCommand : ICommand
    {
        
        public void Execute(CommandArgs args)
        {
            CommandHandler.Send<ConfigureSidePanelCommand>(args);
        }
    }
}