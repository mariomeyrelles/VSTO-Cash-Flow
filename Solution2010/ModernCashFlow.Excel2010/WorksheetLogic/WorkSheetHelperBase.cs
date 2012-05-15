using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using ModernCashFlow.Tools;
using ListObject = Microsoft.Office.Tools.Excel.ListObject;

namespace ModernCashFlow.Excel2010.WorksheetLogic
{
    public abstract class WorksheetHelperBase<TKey, TEntity>
    {
        protected ListObject Table;
        protected WorksheetBase Sheet;
        protected Dictionary<string, int> Cols;
        protected Dictionary<string, int> AbsCols;
        protected Dictionary<TKey, Range> RowIndex;
        protected string TableName;
        protected string[] DatabindCols;

        protected WorksheetHelperBase(WorksheetBase sheet, ListObject table)
        {
            Sheet = sheet;
            Table = table;
            RowIndex = new Dictionary<TKey, Range>();

            TableName = table.Name;
        }

        protected void ReadColumnPositions()
        {
            Cols = new Dictionary<string, int>();
            AbsCols = new Dictionary<string, int>();
            var leftAbsCol = Table.ListColumns.Item[1].Range.Column - 1;

            object[,] columnData = Table.HeaderRowRange.Value2;

            for (var i = 1; i <= columnData.GetLength(1); i++)
            {
                Cols.Add(columnData[1, i].ToString(), i);
                AbsCols.Add(columnData[1, i].ToString(), leftAbsCol + i);
            }

        }

        protected virtual void PrepareColumnsForDatabinding()
        {
            DatabindCols = ExcelUtil.PrepareColumnNamesForDatabinding<TEntity>(Cols.Keys.ToList());
        }


        protected void SetValidationForColumn(IEnumerable<string> items, string columnName)
        {
            var separator = Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
            var values = string.Join(separator, items);
            var range = Sheet.Range[string.Format("{0}[{1}]", TableName, columnName)];

            range.Validation.Delete();
            range.Validation.Add(XlDVType.xlValidateList, XlDVAlertStyle.xlValidAlertInformation, XlFormatConditionOperator.xlBetween, values);
            range.Validation.InCellDropdown = true;
            range.Validation.IgnoreBlank = true;
        }

        /// <summary>
        /// Proteger o table contra modificações.
        /// </summary>
        protected void Protect()
        {
            Sheet.Protect(allowFormattingColumns: true, allowFormattingRows: true, allowSorting: true, allowFiltering: true, allowUsingPivotTables: true);
            Globals.ThisWorkbook.ThisApplication.EnableEvents = true;
        }

        /// <summary>
        /// Desproteger a planilha para modificações
        /// </summary>
        protected void Unprotect(bool enableEvents = false)
        {
            Sheet.Unprotect();
            Globals.ThisWorkbook.ThisApplication.EnableEvents = enableEvents;
        }

        
    }
}