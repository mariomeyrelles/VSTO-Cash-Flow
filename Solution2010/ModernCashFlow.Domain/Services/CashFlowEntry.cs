using System;

namespace ModernCashFlow.Domain.Services
{
    public struct CashFlowEntry : IEquatable<CashFlowEntry>
    {
        public CashFlowEntry(int accountId,  DateTime date, decimal value) : this()
        {
            AccountId = accountId;
            Value = value;
            Date = date;
        }

        public int AccountId { get; private set; }
        public decimal Value { get; private set; }
        public DateTime Date { get; private set; }

        public bool Equals(CashFlowEntry other)
        {
            return other.AccountId == AccountId && other.Value == Value && other.Date.Equals(Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CashFlowEntry && Equals((CashFlowEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = AccountId;
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


        public override string ToString()
        {
            return string.Format("AccountId:{0}; Date: {1}, Value: {2}", AccountId, Date, Value);
        }
    }
}