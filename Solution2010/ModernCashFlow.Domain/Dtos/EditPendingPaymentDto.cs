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
        public EditPendingExpenseDto( Expense s)
        {
            Expense = s;
        }
        public Expense Expense { get; set; }
        public bool IsPaid { get; set; }

        public static List<Expense> ToList(IEnumerable<EditPendingExpenseDto> entities, Func<EditPendingExpenseDto,bool> where)
        {
            return entities.Where(where).Select(x=>x.Expense).ToList();
        }
    }
}