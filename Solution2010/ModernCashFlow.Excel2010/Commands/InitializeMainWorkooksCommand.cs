using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010.Commands
{

    /// <summary>
    /// Responsible to initalize the main worksheets after their dependencies initialization.
    /// </summary>
    public class InitializeMainWorksheetsCommand : ICommand
    {
        private readonly IncomeWorksheet _incomeWorksheet;
        private readonly ExpenseWorksheet _expenseWorksheet;

        public InitializeMainWorksheetsCommand(IncomeWorksheet incomeWorksheet, ExpenseWorksheet expenseWorksheet)
        {
            _expenseWorksheet = expenseWorksheet;
            _incomeWorksheet = incomeWorksheet;
        }

        public void Execute(CommandArgs args)
        {
            //initalize other worksheet helpers
            _incomeWorksheet.Start();
            _expenseWorksheet.Start();
        }
    }

}