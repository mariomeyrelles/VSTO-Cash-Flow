using ModernCashFlow.Excel2010.Forms;

namespace ModernCashFlow.Excel2010.Commands
{
    public class ConfigureSidePanelCommand : ICommand
    {
        private readonly SidePanelWpfHost _host;

        public ConfigureSidePanelCommand()
        {
            _host = new SidePanelWpfHost();
            Globals.ThisWorkbook.ActionsPane.Controls.Add(_host);

            //solicitar o refresh do host do wpf sempre que o panel mudar de tamanho ou acontecer algum scroll.
            Globals.ThisWorkbook.ActionsPane.Resize += delegate { _host.Refresh(); };
            Globals.ThisWorkbook.ActionsPane.Scroll += delegate { _host.Refresh(); };

        }
        public void Execute(CommandArgs args)
        {
            var sidePanelArg = args as SidePanelCommandArgs;
            if (sidePanelArg == null)
            {
                return;
            }
            if (sidePanelArg.WpfControl != null) _host.CurrentControl = sidePanelArg.WpfControl;
            if (sidePanelArg.Model != null) _host.Model = sidePanelArg.Model;

            _host.Refresh();
        }
    }

   
}