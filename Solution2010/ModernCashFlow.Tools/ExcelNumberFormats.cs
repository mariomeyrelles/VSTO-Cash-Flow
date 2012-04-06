namespace ModernCashFlow.Tools
{
    /// <summary>
    /// Contém constantes para ajudar a formatar colunas em formatos padrão do Excel.
    /// </summary>
    public static class ExcelNumberFormats
    {
        public const string Accounting = "_($* #,##0.00_);_($* (#,##0.00);_($* '-'??_);_(@_)";
        public const string Currency = "$ #,##0.00";
        public const string Text = "@";
    }
}