using System;
using ModernCashFlow.Domain.BaseInterfaces;

namespace ModernCashFlow.Domain.Entities
{
    public class Account : DomainBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResponsibleName { get; set; }
        public decimal? InitialBalance { get; set; }
        public DateTime? InitialDate { get; set; }
        public bool AcceptsDeposits { get; set; }
        public bool AcceptsManualAdjustment { get; set; }
        public bool AcceptsNegativeValues { get; set; }
        public bool AcceptsRecharge { get; set; }
        public bool RequiresPayment { get; set; }
        public bool AcceptsPartialPayment { get; set; }
        public bool AcceptsLatePaymentInterest { get; set; }
        public bool AcceptsYield { get; set; }
        public bool AcceptsChecks { get; set; }
        public int? CloseDay { get; set; }
        public int? PaymentDay { get; set; }
        public decimal? MonthlyCost { get; set; }

        public double? InitialDateOA
        {
            get
            {
                var dateTime = this.InitialDate;
                if (dateTime != null) return dateTime.Value.ToOADate();
                return null;
            }
        }
       
    }
}