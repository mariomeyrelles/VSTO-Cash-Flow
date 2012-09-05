using System;
using System.ComponentModel;
using ModernCashFlow.Domain.BaseInterfaces;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Entities
{
    public class Income : BaseTransaction, IMoneyTransaction
    {
        public Income()
        {
            this.PropertyChanged += OnPropertyChanged;
        }

        public Income(Guid identity)
        {
            this.TransactionCode = identity;
            this.PropertyChanged += OnPropertyChanged;
        }


        public override decimal Value
        {
            get
            {
                //if (this.ActualValue.HasValue)
                //{
                //    return this.ActualValue.Value;
                //}
                return this.ExpectedValue ?? 0.0m;
            }
        }


        public override string ToString()
        {
            return string.Format("(Income) AccountId:{0}; Date: {1}, Value: {2}", this.AccountId, this.Date, this.Value);
        }
    }
}