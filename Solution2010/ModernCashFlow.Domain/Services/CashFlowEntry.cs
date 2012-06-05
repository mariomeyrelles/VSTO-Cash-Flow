using System;

namespace ModernCashFlow.Domain.Services
{
    public struct CashFlowEntry : IEquatable<CashFlowEntry>
    {
        public int AccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

        public bool Equals(CashFlowEntry other)
        {
            return other.AccountId == AccountId && other.Value == Value && other.Date.Equals(Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (CashFlowEntry)) return false;
            return Equals((CashFlowEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = AccountId;
                result = (result*397) ^ Value.GetHashCode();
                result = (result*397) ^ Date.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(CashFlowEntry left, CashFlowEntry right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(CashFlowEntry left, CashFlowEntry right)
        {
            return !left.Equals(right);
        }

    }
}