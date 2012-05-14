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
        public double InitialBalance { get; set; }
        public DateTime InitialDate { get; set; }
        public bool AcceptsDeposits { get; set; }
        public bool AcceptsManualAdjustment { get; set; }
        public bool AcceptsNegativeValues { get; set; }
        public bool AcceptsRecharge { get; set; }
        public bool RequiresPayment { get; set; }
        public bool AcceptsPartialPayment { get; set; }
        public bool AcceptsInterest { get; set; }
        public bool AcceptsYield { get; set; }
        public bool AcceptsChecks { get; set; }
        public int CloseDay { get; set; }
        public int PaymentDay { get; set; }
        public double MonthlyCost { get; set; }

    }
}