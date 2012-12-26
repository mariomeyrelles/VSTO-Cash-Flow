using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// This controller manages the Income entity.
    /// </summary>
    public class IncomeController : BaseController<Income>
    {
        public override void GetLocalDataAndSyncronizeSession()
        {
            base.GetLocalDataAndSyncronizeSession();

            foreach (var income in SessionData)
            {
                income.AccountId = CurrentSession.Accounts.First(x => x.Name == income.AccountName).Id;
            }
        }

        /// <summary>
        /// Tells the controller to accept a new entity from anywhere and sync it to the session.
        /// </summary>
        /// <param name="localData">The entity received to be synchronized</param>
        /// <param name="notifyChange">When true, this new entity must be sent to all clients. Default is false.</param>
        public override void AcceptData(Income localData, bool notifyChange = false)
        {
            var index = SessionData.FindIndex(x => x.TransactionCode == localData.TransactionCode);
            if (index < 0)
                SessionData.Add(localData);
            else
                SessionData[index] = localData;

            if (!string.IsNullOrEmpty(localData.AccountName))
            {
                localData.AccountId = CurrentSession.Accounts.First(x => x.Name == localData.AccountName).Id; 
            }
            
            
            if (notifyChange)
            {
                localData.NotifyPropertyChange();
                RefreshSingleLocalData(localData);
            }
        }


        /// <summary>
        /// Tells the controller to accept a new collection of entities from anywhere and sync them to the session.
        /// </summary>
        /// <param name="localData">The collection received to be synchronized</param>
        /// <param name="notifyChange">When true, this new entity must be sent to all clients. Default is false.</param>
        public override void AcceptDataCollection(IEnumerable<Income> localData, bool notifyChange = false)
        {
            //todo: check if performance is acceptable.
            foreach (var entity in localData)
            {
                this.AcceptData(entity, notifyChange);
            }
        }

        

        
        
    }
}