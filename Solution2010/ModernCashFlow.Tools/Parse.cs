using System;
using System.Globalization;
using Microsoft.Office.Interop.Excel;

namespace ModernCashFlow.Tools
{
    /// <summary>
    /// Contém métodos que auxiliam a ler valores dinâmicos lidos de uma célula do Excel.
    /// </summary>
    public static class Parse
    {
        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados DateTime. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um DateTime ou null em caso de falha.</returns>
        public static DateTime? ToDateTime(dynamic data)
        {
            if (data is double)
            {
                return DateTime.FromOADate(data);
            }

            var c = CultureInfo.CurrentUICulture;

            if (data == null) return null;

            var value = data.ToString();
            if (value == null) return null;

            DateTime output;

            return DateTime.TryParse(value, c, DateTimeStyles.None, out output) ? (DateTime?)output : null;
        }

       
        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados Int32. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um Int32 ou null em caso de falha.</returns>
        public static int? ToInt(dynamic data)
        {
            if (data == null) return null;

            var input = data.ToString();
            if (string.IsNullOrEmpty(input)) return null;

            int value;
            return int.TryParse(input, out value) ? (int?)value : null;
        }

        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados Boolean. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um Boolean ou null em caso de falha.</returns>
        public static bool? ToBoolean(dynamic data)
        {
            if (data == null) return null;

            var input = data.ToString();
            if (string.IsNullOrEmpty(input)) return null;

            bool value;
            return bool.TryParse(input, out value) ? (bool?)value : null;
        }

        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados Double. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um Double ou null em caso de falha.</returns>
        public static double? ToDouble(dynamic data)
        {
            if (data == null) return null;
            var input = data.ToString();
            if (string.IsNullOrEmpty(input)) return null;

            double value;
            return double.TryParse(input, out value) ? (double?)value : null;
        }

        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados DateTime. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um DateTime ou null em caso de falha.</returns>
        public static decimal? ToDecimal(dynamic data)
        {
            if (data == null) return null;
            var input = data.ToString();
            if (string.IsNullOrEmpty(input)) return null;

            decimal value;
            return decimal.TryParse(input, out value) ? (decimal?)value : null;
        }



        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados String. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um String ou null em caso de falha.</returns>
        public static string ToString(dynamic data)
        {
            if (data is string) return data;

            if (data == null) return null;

            return data.ToString();
        }

        /// <summary>
        /// Tenta converter o conteúdo de um range no tipo de dados Guid. Caso a conversão não dê certo, retorna null.
        /// </summary>
        /// <param name="data">Célula com o conteúdo a ser lido.</param>
        /// <returns>Retorna um Guid ou null em caso de falha.</returns>
        public static Guid? ToGuid(dynamic data)
        {
            if (data == null ) return null;
            var input = data.ToString();
            if (string.IsNullOrEmpty(input)) return null;

            Guid value;
            return Guid.TryParse(input, out value) ? (Guid?)value : null;
        }
       
    }
}