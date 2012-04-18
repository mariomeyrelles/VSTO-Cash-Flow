using System;
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
        /// Responsável por montar uma lista de colunas para usar com o databinding do ListObject. Não posso usar simplesmente as 
        /// descrições. É preciso usar os nomes físicos das propriedades.
        /// </summary>
        /// <param name="columnNames"></param>
        /// <typeparam name="T"></typeparam>
        public static string[] PrepareColumnNamesForDatabinding<T>(IEnumerable<string> columnNames)
        {

            var columnIds = new List<string>();

            //look for properties with the Localizable Description attribute.
            var props = from x in typeof(T).GetProperties()
                        select new 
                            {   Description = x.GetCustomAttributes(false).OfType<LocalizableColumnNameAttribute>().FirstOrDefault(),
                                PropertyName =  x.Name,
                                PropertyType = x.GetType()
                            };
            
            //Consider the resource key to be same 
            var localizedDescriptions = from x in props.Where(x => x.Description != null)
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

            for (int i = 0; i < columnIds.Count; i++)
            {
                //this solution works. Write a double value using FromOADate(datetime).
                if (columnIds[i] == "Date")
                {
                    columnIds[i] = columnIds[i] + "_OA";
                }
            }

            return columnIds.ToArray();

        }
    }
}