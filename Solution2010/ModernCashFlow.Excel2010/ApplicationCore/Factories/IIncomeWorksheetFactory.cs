using ModernCashFlow.Excel2010.WorksheetLogic;

namespace ModernCashFlow.Excel2010.ApplicationCore.Factories
{
    /// <summary>
    /// Abstract Factory for creating Worksheet logic objects. Meant to be used with Ninject Factory extension.
    /// </summary>
    public interface IIncomeWorksheetFactory
    {
        IncomeWorksheet CreateWorksheet();
        IncomeWorksheet.ContextMenus CreateContextMenu();
        IncomeWorksheet.Events CreateEventHandlers();
    }
}