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

            /*
             * First step: Receive the actual column name of each column in the worksheet - the can't be changed by the user.
             * Second step: find each property marked with [LocalizableDescription]
             * Third step: use each property found as a key - this key will look for a matching resource name in resource name and bring the localized description for the current language.
             * Fourth step: match the descriptions found in resource manager with the received column names from the worksheet. 
             * Fifth step: When dealing with internationalized workbooks, you must write dates as double values - it's the internal way excel save dates. I've created some shadow properties with the suffix _OA
             *  in each entity to return the Ole Automation Date representation of a DateTime in .NET (DateTime.FromOADate). In this step, I use the convention PropertyName_OA and change the property names accordingly.
             * Sixth step: return the array with the column names that afterwards shall be used for the ListObject databinding system.
             */

            var colIds = from c in columnNames
                         //look for properties with the Localizable Description attribute. Ignore other properties.
                         let props = from x in typeof(T).GetProperties()
                                     select new {
                                                    Description = x.GetCustomAttributes(false).OfType<LocalizableColumnNameAttribute>().FirstOrDefault(),
                                                    PropertyName = x.Name,
                                                    PropertyType = x.PropertyType
                                                }
                         //get the localized descriptions for each property market with [LocalizableDescription]
                         let localizedDescriptions = from x in props.Where(x => x.Description != null)
                                                     select new {
                                                                     ResourceKey = x.PropertyName,
                                                                     LocalizedDescr = Lang.ResourceManager.GetString(x.PropertyName),
                                                                     PropertyType = x.PropertyType
                                                                }
                         from l in localizedDescriptions
                         where c == l.LocalizedDescr
                         select new ColumnData { ColumnName = l.ResourceKey, ColumnType = l.PropertyType };

            foreach (var columnData in colIds)
            {
                if (columnData.ColumnType == typeof (DateTime) || columnData.ColumnType == typeof (DateTime?))
                {
                    //todo: maybe it's worth spending some time trying to crate a "OA" version of each DateTime property using IL.
                    columnData.SetColumnName(columnData.ColumnName + "_OA");
                }

                columnIds.Add(columnData.ColumnName);
            }

            return columnIds.ToArray();

        }
    }

    struct ColumnData
    {
        public string ColumnName { get; set; }

        public Type ColumnType { get; set; }

        public void SetColumnName(string newColumnName)
        {
            this.ColumnName = newColumnName;
        }
    }
}