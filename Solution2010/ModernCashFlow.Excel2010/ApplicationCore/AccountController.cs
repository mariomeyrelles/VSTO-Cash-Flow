using System;
using System.Collections.Generic;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    public class AccountController : BaseController<Account>
    {
        public override void AcceptData(Account localData, bool notifyChange = false)
        {
            var index = SessionData.FindIndex(x => x.Id == localData.Id);
            if (index < 0)
                SessionData.Add(localData);
            else
                SessionData[index] = localData;

            if (notifyChange)
            {
                localData.NotifyPropertyChange();
                RefreshSingleLocalData(localData);
            }
        }

        public override void AcceptDataCollection(IEnumerable<Account> localData, bool notifyChange = false)
        {
            foreach (var item in localData)
            {
                this.AcceptData(item, notifyChange);
            }
        }
    }
}