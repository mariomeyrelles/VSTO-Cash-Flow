using System;
using System.Linq;
using System.Text;

namespace ModernCashFlow.Domain.Services
{
    public struct BalanceEntry : IEquatable<BalanceEntry>
    {
        public BalanceEntry(int accountId, decimal value)
            : this()
        {
            Value = value;
            AccountId = accountId;
        }

        public int AccountId { get; private set; }
        public decimal Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(BalanceEntry)) return false;
            return Equals((BalanceEntry)obj);
        }

        public bool Equals(BalanceEntry other)
        {
            return other.AccountId == AccountId && other.Value == Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (AccountId * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(BalanceEntry left, BalanceEntry right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BalanceEntry left, BalanceEntry right)
        {
            return !left.Equals(right);
        }
    }
}
