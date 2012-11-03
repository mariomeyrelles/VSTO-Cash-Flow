using System;
using System.ComponentModel;
using ModernCashFlow.Globalization.Resources;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Entities
{
    /// <summary>
    /// The Expense domain entity, which refers to money getting out.
    /// </summary>
    public class Expense : BaseTransaction, IMoneyTransaction
    {
        public Expense()
        {
            this.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        public Expense(Guid identity)
        {
            this.TransactionCode = identity;
            this.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }


        public override decimal Value
        {
            get
            {
                if (this.ActualValue.HasValue)
                {
                    return this.ActualValue.Value * -1.0m;
                }
                return this.ExpectedValue * -1.0m ?? 0.0m;
            }
        }


        public override string ToString()
        {
            return string.Format("(Expense) AccountId:{0}; Date: {1}, Value: {2}", this.AccountId, this.Date, this.Value);
        }
    }
}