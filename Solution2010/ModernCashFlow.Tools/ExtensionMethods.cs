using System;
using System.ComponentModel;
using System.Reflection;

namespace ModernCashFlow.Tools
{
    public static class RangeExtensionMethods
    {

        //todo: review where these methods should be placed
        public static DateTime Today(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, data.Day);
        }

        public static DateTime Today(this double data)
        {
            return DateTime.FromOADate(data);
        }

        public static DateTime? Today(this DateTime? data)
        {
            if (data.HasValue)
            {
                return new DateTime(data.Value.Year, data.Value.Month, data.Value.Day);
            }

            return null;
         
        }

        public static DateTime Today(this DateTime? data, DateTime defaultDateTime)
        {
            return data.HasValue ? new DateTime(data.Value.Year, data.Value.Month, data.Value.Day) : defaultDateTime;
        }


        public static dynamic ToInt(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return Convert.ToInt32(input);
        }

        public static dynamic ToDate(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return Convert.ToDateTime(input);
        }

        public static dynamic ToBoolean(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return Convert.ToBoolean(input);
        }

        public static dynamic ToDouble(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            return Convert.ToDouble(input);
        }
    }

    public static class CoreTypesExtensions
    {
        public static bool Invert(this bool value)
        {
            return !value;
        }
    }

    public static class EnumTools
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
           // or throw new ArgumentException("Not found.", "description");
            return default(T);
        }

    }
}

