using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.CSharp.RuntimeBinder;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.WpfControls.Controls
{
    public class MultiStatusDataTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (item == null) return null;
            var status = (item as dynamic).TransactionStatus;
            if ((status is TransactionStatus) == false) return null;

            try
            {
                var element = container as FrameworkElement;

                if (element != null)
                {
                    switch ((TransactionStatus)status)
                    {
                       
                        case TransactionStatus.Pending:
                            return element.FindResource("notOkay") as DataTemplate;
                        case TransactionStatus.OK:
                        case TransactionStatus.Scheduled:
                            return element.FindResource("okay") as DataTemplate;
                        case TransactionStatus.Suspended:
                        case TransactionStatus.Canceled:
                        case TransactionStatus.Invalid:
                        case TransactionStatus.Unknown:
                            return element.FindResource("unknown") as DataTemplate;
                    }
                }
            }
            catch (RuntimeBinderException)
            {
                return null;
            }

            return null;
        }
    }
}