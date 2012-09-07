using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010.Commands
{
    /// <summary>
    /// Responsible to read the configuration stored in configuration worksheets. With this data,
    /// the main application build validation lists, environment configuration and so on.
    /// </summary>
    public class InitializeBasicBusinessDependenciesCommand : ICommand
    {
        private readonly BaseController<Account> _accountController;
        private readonly AccountWorksheet _accountWorksheet;


        public InitializeBasicBusinessDependenciesCommand(AccountWorksheet accountWorksheet, BaseController<Account> accountController)
        {
            _accountWorksheet = accountWorksheet;
            _accountController = accountController;
        }

        public void Execute(CommandArgs args)
        {
            _accountWorksheet.Start();
            _accountController.GetLocalDataAndSyncronizeSession();
        }
    }
}