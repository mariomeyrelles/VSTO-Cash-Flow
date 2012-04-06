using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ModernCashFlow.Domain.BaseInterfaces
{
    /// <summary>
    /// Base domain logic.
    /// </summary>
    public abstract class DomainBase : IDomainObject, INotifyPropertyChanged
    {
        
        #region INotifyPropertyChanged implementations

        public event PropertyChangedEventHandler PropertyChanged;

        //Note: INPC implementation found at: http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist


        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            var body = selectorExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("The body must be a member expression");
            OnPropertyChanged(body.Member.Name);
        }

        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> selectorExpression)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(selectorExpression);
            return true;
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public virtual void NotifyPropertyChange()
        {

        }

        
        #endregion

    }
}