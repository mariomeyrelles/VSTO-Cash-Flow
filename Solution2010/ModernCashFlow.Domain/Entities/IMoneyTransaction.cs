using System;

namespace ModernCashFlow.Domain.Entities
{
    public interface IMoneyTransaction
    {
        
        int AccountID { get; set; }

        DateTime? Date { get; set; }

        decimal Value { get; }
    }
}