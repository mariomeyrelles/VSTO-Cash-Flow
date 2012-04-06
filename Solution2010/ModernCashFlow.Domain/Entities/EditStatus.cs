using System.ComponentModel;

namespace ModernCashFlow.Domain.Entities
{
    /// <summary>
    /// Edit status used to control how complete a transaction is.
    /// </summary>
    public enum EditStatus
    {
        /// <summary>
        /// When tje transaction is not saved yet.
        /// </summary>
        Created,
        /// <summary>
        /// When the transaction is not ready to be used in the cash flow.
        /// </summary>
        Incomplete,

        /// <summary>
        /// When the transaction is ok to be used.
        /// </summary>
        Complete,


        //todo: maybe it's necessary to create a "partially complete" status
    }
}