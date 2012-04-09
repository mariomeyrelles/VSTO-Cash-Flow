using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ModernCashFlow.Tools
{
    public class Util
    {
        /// <summary>
        /// Retorna as descrições de todos os itens de um Enum em formato de lista utilizando o atributo Description de cada item da Enum.
        /// Caso o tipo passado não seja uma enum, retorna vazio.
        /// </summary>
        /// <param name="enumType">O tipo da enum.</param>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions(Type enumType)
        {
            if (enumType.BaseType != typeof(Enum))
            {
                return null;
            }

            var itens = new List<string>();
            FieldInfo fi;
            DescriptionAttribute da;
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                fi = enumType.GetField((enumValue.ToString()));
                da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if (da != null)
                {
                    itens.Add(da.Description);
                }
            }
            return itens;
        }


    }
}