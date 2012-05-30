using System;

namespace ModernCashFlow.Domain.Entities
{
    public interface IMoneyTransaction
    {
        
        int AccountId { get; set; }

        DateTime? Date { get; set; }

        decimal Value { get; }
    }
}