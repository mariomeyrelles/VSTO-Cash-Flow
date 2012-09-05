using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010.Commands
{
    //todo: pass reference via constructor.


    public class InitializeBasicDependenciesCommand : ICommand
    {
        [Inject]
        public BaseController<Account> AccountController { get; set; }

        public void Execute(CommandArgs args)
        {
            NinjectContainer.Kernel.Get<AccountWorksheet>().Start();
            AccountController.GetLocalDataAndSyncronizeSession();
        }
    }
}