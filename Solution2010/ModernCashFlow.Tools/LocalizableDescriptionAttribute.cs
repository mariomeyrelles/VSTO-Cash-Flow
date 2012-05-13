using System;
using System.ComponentModel;
using System.Globalization;
using ModernCashFlow.Globalization.Resources;

namespace ModernCashFlow.Tools
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class LocalizableDescriptionAttribute : DescriptionAttribute
    {

        private readonly string _resourceKey;

        public LocalizableDescriptionAttribute()
        {
        }
        

        public LocalizableDescriptionAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        

        public override string Description
        {
            get
            {
                //todo: lançar erro quando não encontrar recurso
                return this.DescriptionValue = Lang.ResourceManager.GetString(_resourceKey, CultureInfo.CurrentUICulture);
            }
        }

    }
}