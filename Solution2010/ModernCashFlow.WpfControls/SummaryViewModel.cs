using System;
using System.ComponentModel;
using ModernCashFlow.Domain.ApplicationServices;

namespace ModernCashFlow.WpfControls
{
    public class SummaryViewModel : ViewModelBase, IObserver<MainStatusAppService>, INotifyPropertyChanged
    {
        private decimal _expensesUpToDate;
        private decimal _endOfMonthGeneralBalance;
        private decimal _incomesUpToDate;

        public SummaryViewModel()
        {
            Singleton<MainStatusAppService>.Instance.Subscribe(this);
        }

        public decimal ExpensesUpToDate
        {
            get { return _expensesUpToDate; }
            set
            {
                _expensesUpToDate = value;
                OnPropertyChanged("ExpensesUpToDate");
            }
        }

        public decimal EndOfMonthGeneralBalance
        {
            get { return _endOfMonthGeneralBalance; }
            set
            {
                _endOfMonthGeneralBalance = value;
                OnPropertyChanged("EndOfMonthGeneralBalance");
            }
        }

        public decimal IncomesUpToDate
        {
            get { return _incomesUpToDate; }
            set
            {
                _incomesUpToDate = value;
                OnPropertyChanged("IncomesUpToDate");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }






        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion



        #region IObserver<MainStatusAppService> Members

        public void OnNext(MainStatusAppService value)
        {
            this.ExpensesUpToDate = value.ExpensesUpToDate;
            this.IncomesUpToDate = value.IncomesUpToDate;
            this.EndOfMonthGeneralBalance = value.EndOfMonthBalance;
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        #endregion







       
    }
}