


using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools;
using ModernCashFlow.Domain.ApplicationServices;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.Forms;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Excel2010.Commands
{
    public class ConfigureSidePanelCommand : ICommand
    {
        private  SidePanelWpfHost _host;
        private  ActionsPane _sidePane;

        public ConfigureSidePanelCommand()
        {
            _host = new SidePanelWpfHost();
            _sidePane = Globals.ThisWorkbook.ActionsPane;
            _sidePane.Controls.Add(_host);

            //solicitar o refresh do host do wpf sempre que o panel mudar de tamanho ou acontecer algum scroll.
            _sidePane.Resize += delegate { _host.Refresh();
                                           _host.Height = _sidePane.Height;
            };

            _sidePane.Scroll += delegate { _host.Refresh(); };

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
            _host.Show();


            //todo: only for tests
            var svc = new SummaryCalculationService();
            var currentMonthBalance = svc.CalculateBalanceForCurrentMonth(sidePanelArg.Transactions);
            var incomesUpToDate = svc.CalculateIncomesForCurrentMonthUpToGivenDate(sidePanelArg.Transactions, SystemTime.Now());
            var expensesUpToDate = svc.CalculateExpensesForCurrentMonthUpToGivenDate(sidePanelArg.Transactions, SystemTime.Now());
            var accountSummary = svc.CalculateAccountSummary(sidePanelArg.Accounts,
                                                             sidePanelArg.Transactions);

            Singleton<MainStatusAppService>.Instance.EndOfMonthBalance = currentMonthBalance;
            Singleton<MainStatusAppService>.Instance.IncomesUpToDate = incomesUpToDate;
            Singleton<MainStatusAppService>.Instance.ExpensesUpToDate = expensesUpToDate;
            Singleton<MainStatusAppService>.Instance.AccountSummary = accountSummary;
        }
    }

   
}