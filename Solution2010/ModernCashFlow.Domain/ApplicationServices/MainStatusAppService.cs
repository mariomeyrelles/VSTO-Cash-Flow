using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;


namespace ModernCashFlow.Domain.ApplicationServices
{
    public class MainStatusAppService : IObservable<MainStatusAppService>
    {
       
        public MainStatusAppService()
        {
            Observers = new List<IObserver<MainStatusAppService>>();

            var timer = new Timer(3000);
            timer.Elapsed += TimerElapsed;
            timer.Start();

        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Notify();
        }

        public decimal IncomesUpToDate { get; set; }

        public decimal ExpensesUpToDate { get; set; }

        public decimal EndOfMonthBalance { get; set; }
       
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.OnNext(this);
            }
        }


        public List<IObserver<MainStatusAppService>>  Observers { get; set; }
        
        public IDisposable Subscribe(IObserver<MainStatusAppService> observer)
        {
            if (!Observers.Contains(observer))
                Observers.Add(observer);
            return new Unsubscriber(Observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<MainStatusAppService>> _observers;
            private readonly IObserver<MainStatusAppService> _observer;

            public Unsubscriber(List<IObserver<MainStatusAppService>> observers, IObserver<MainStatusAppService> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}