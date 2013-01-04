namespace ModernCashFlow.Excel2010.Commands
{
    public class ShowSidePanelCommand : ICommand
    {
        public void Execute(CommandArgs args)
        {
            Globals.ThisWorkbook.Application.CommandBars["Task Pane"].Visible = true;
        }
    }
}