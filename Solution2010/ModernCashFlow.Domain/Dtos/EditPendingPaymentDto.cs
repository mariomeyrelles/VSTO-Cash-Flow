using ModernCashFlow.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ModernCashFlow.Domain.Dtos
{
    /// <summary>
    /// Dto used in the Pending Expenses screen.
    /// </summary>
    public class EditPendingExpenseDto
    {
        public EditPendingExpenseDto( BaseTransaction transaction)
        {
            Transaction = transaction;
        }
        public BaseTransaction Transaction { get; set; }
        public bool IsOk { get; set; }

        public static List<BaseTransaction> ToList(IEnumerable<EditPendingExpenseDto> entities, Func<EditPendingExpenseDto,bool> where)
        {
            return entities.Where(where).Select(x=>x.Transaction).ToList();
        }
    }
}