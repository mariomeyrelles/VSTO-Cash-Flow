using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using ModernCashFlow.Domain.BaseInterfaces;
using ModernCashFlow.Domain.Entities;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Controller logic, responsible to manage in/out flows of data from different parts of the application.
    /// </summary>
    /// <typeparam name="T">A domain entity</typeparam>
    public abstract class BaseController<T> where T : DomainBase
    {
        /// <summary>
        /// Occours when the all local worksheet data needs to be refreshed with a new set of data.
        /// </summary>
        public event Action<IEnumerable<T>> UpdateAllLocalData;

        /// <summary>
        /// Occours when only a single line of data needs to be refreshed on the local worksheet.
        /// </summary>
        public event Action<T> UpdateSingleLocalData;


        /// <summary>
        /// Occours when the application needs to send all the local data to the remote storage.
        /// </summary>
        public event Action<IEnumerable<T>> UpdateRemoteData;

        
        /// <summary>
        /// Occours when the application needs to get all data in the worksheet and store it in the session memory for later use.
        /// </summary>
        public event Func<IEnumerable<T>> RetrieveAllLocalData;

        /// <summary>
        /// Occours when the application needs to get all data from remote storage and put it in session memory for later use.
        /// </summary>
        public event Func<IEnumerable<T>> RetrieveAllRemoteData;

        /// <summary>
        /// The current session data for this controller.
        /// </summary>
        protected List<T> SessionData
        {
            get { return SessionDataSingleton<T>.Instance; }
        }

        /// <summary>
        /// A read-only representation of current session data for public use.
        /// </summary>
        public IEnumerable<T> CurrentSessionData
        {
            get { return SessionDataSingleton<T>.Instance.AsReadOnly(); }
        }

       
        /// <summary>
        /// Event handler for RetrieveAllRemoteData.
        /// </summary>
        /// <returns>The data received from the remote storage.</returns>
        protected IEnumerable<T> OnRetrieveRemoteData()
        {
            if (RetrieveAllRemoteData != null)
            {
                var dados = RetrieveAllRemoteData();
                return dados;
            }
            return null;
        }

        /// <summary>
        /// Event handler for UpdateRemoteData event.
        /// </summary>
        /// <param name="newData">The new data set to be sent to remote storage.</param>
        protected void OnUpdateRemoteData(IEnumerable<T> newData)
        {
            if (UpdateRemoteData != null)
                UpdateRemoteData(newData);
        }

        /// <summary>
        /// Event handler for the RetrieveLocalData event.
        /// </summary>
        /// <returns>This is the data received from the worksheet, which is then used by the controller to update session data.</returns>
        protected virtual IEnumerable<T> OnRetrieveLocalData()
        {
            if (RetrieveAllLocalData != null)
            {
                var dados = RetrieveAllLocalData();
                return dados;
            }
            return null;
        }

        /// <summary>
        /// Event handler for UpdateAllLocalData event.
        /// </summary>
        /// <param name="newData">The whole data which will be sent to the worksheet.</param>
        protected virtual void OnUpdateAllLocalData(IEnumerable<T> newData)
        {
            if (UpdateAllLocalData != null)
                UpdateAllLocalData(newData);
        }

        //todo: criar método para atualizar apenas um conjunto de linhas.

        /// <summary>
        /// Event handler for the UpdateSingleLocalData event. 
        /// </summary>
        /// <param name="newData">The single entity to be sent to the worksheet.</param>
        protected virtual void OnUpdateSingleLocalData(T newData)
        {
            if (UpdateSingleLocalData != null)
                UpdateSingleLocalData(newData);
        }


        /// <summary>
        /// Retrieves all data from the worksheet, clears the session and update the session with this received data.
        /// </summary>
        public virtual void GetLocalDataAndSyncronizeSession()
        {
            var localData = OnRetrieveLocalData();
            
            if (localData == null)
                return;
            
            var dataMgr = SessionDataSingleton<T>.Instance;
            dataMgr.Clear();
            dataMgr.AddRange(localData);
        }

        /// <summary>
        /// Requests a complete update of all local worksheet data.
        /// </summary>
        public virtual void RefreshAllLocalData()
        {
            var memoryData = SessionDataSingleton<T>.Instance;
            OnUpdateAllLocalData(memoryData);
        }


        /// <summary>
        /// Requests a single worksheet row with a given entity instance.
        /// </summary>
        /// <param name="localData">The single entity to be sent to the worksheet as new data.</param>
        public virtual void RefreshSingleLocalData(T localData)
        {
            OnUpdateSingleLocalData(localData);
        }

        /// <summary>
        /// Tells the controller to accept a new entity from anywhere and sync it to the session.
        /// </summary>
        /// <param name="localData">The entity received to be synchronized</param>
        /// <param name="notifyChange">When true, this new entity must be sent to all clients. Default is false.</param>
        public abstract void AcceptData(T localData, bool notifyChange = false);

        /// <summary>
        /// Tells the controller to accept a new collection of entities from anywhere and sync them to the session.
        /// </summary>
        /// <param name="localData">The collection received to be synchronized</param>
        /// <param name="notifyChange">When true, this new entity must be sent to all clients. Default is false.</param>
        public abstract void AcceptDataCollection(IEnumerable<T> localData, bool notifyChange = false);


    }
}