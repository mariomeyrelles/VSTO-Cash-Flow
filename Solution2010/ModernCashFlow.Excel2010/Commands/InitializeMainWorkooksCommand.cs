using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010.Commands
{
    public class InitializeMainWorkooksCommand : ICommand
    {
        [Inject]
        public BaseController<Account> AccountController { get; set; }

        public void Execute(CommandArgs args)
        {
            //initalize other worksheet helpers
            NinjectContainer.Kernel.Get<IncomeWorksheet>().Start();
            NinjectContainer.Kernel.Get<ExpenseWorksheet>().Start();
        }
    }

}