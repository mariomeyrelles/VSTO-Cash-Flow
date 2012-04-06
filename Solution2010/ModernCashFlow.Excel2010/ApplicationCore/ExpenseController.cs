using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Controller responsible to manage the Expense entity.
    /// </summary>
    public class ExpenseController : BaseController<Expense>
    {
        /// <summary>
        /// Tells the controller to accept a new entity from anywhere and sync it to the session.
        /// </summary>
        /// <param name="localData">The entity received to be synchronized</param>
        /// <param name="notifyChange">When true, this new entity must be sent to all clients. Default is false.</param>
        public override void AcceptData(Expense localData, bool notifyChange = false)
        {
            var index = SessionData.FindIndex(x => x.TransactionCode == localData.TransactionCode);
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


        /// <summary>
        /// Tells the controller to accept a new collection of entities from anywhere and sync them to the session.
        /// </summary>
        /// <param name="localData">The collection received to be synchronized</param>
        /// <param name="notifyChange">When true, this new entity must be sent to all clients. Default is false.</param>
        public override void AcceptDataCollection(IEnumerable<Expense> localData, bool notifyChange = false)
        {
            //todo: verificar se a performance é aceitável.
            foreach (var saida in localData)
            {
                this.AcceptData(saida,notifyChange);
            }
        }
        
    }
}