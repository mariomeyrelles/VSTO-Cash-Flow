using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using ModernCashFlow.Globalization.Resources;

namespace ModernCashFlow.Tools
{
    public class ExcelUtil
    {
        /// <summary>
        /// Respons�vel por montar uma lista de colunas para usar com o databinding do ListObject. N�o posso usar simplesmente as 
        /// descri��es. � preciso usar os nomes f�sicos das propriedades.
        /// </summary>
        /// <param name="columnNames"></param>
        /// <typeparam name="T"></typeparam>
        public static string[] PrepareColumnNamesForDatabinding<T>(IEnumerable<string> columnNames)
        {

            var columnIds = new List<string>();

            //look for properties with the Localizable Description attribute.
            var props = from x in typeof(T).GetProperties()
                        select new 
                            {   Descriptions = x.GetCustomAttributes(false).OfType<LocalizableColumnNameAttribute>().FirstOrDefault(),
                                PropertyName =  x.Name 
                            };
            
            //Consider the resource key to be same 
            var localizedDescriptions = from x in props.Where(x => x.Descriptions != null)
                                        select new
                                                {
                                                    ResourceKey = x.PropertyName,
                                                    LocalizedDescr = MainResources.ResourceManager.GetString(x.PropertyName)
                                                };


            var colId = from c in columnNames
                        from l in localizedDescriptions
                        where c == l.LocalizedDescr
                        select l.ResourceKey;

            
            columnIds.AddRange(colId);

            return columnIds.ToArray();

        }
    }
}