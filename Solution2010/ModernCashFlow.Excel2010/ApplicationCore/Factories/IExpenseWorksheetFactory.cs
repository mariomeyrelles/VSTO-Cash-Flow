using ModernCashFlow.Excel2010.WorksheetLogic;

namespace ModernCashFlow.Excel2010.ApplicationCore.Factories
{

    /// <summary>
    /// Abstract Factory for creating Worksheet logic objects. Meant to be used with Ninject Factory extension.
    /// </summary>
    public interface IExpenseWorksheetFactory
    {
        ExpenseWorksheet CreateWorksheet();
        ExpenseWorksheet.ContextMenus CreateContextMenu();
        ExpenseWorksheet.Events CreateEventHandlers();
    }
}