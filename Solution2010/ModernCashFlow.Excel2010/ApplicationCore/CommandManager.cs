using System;
using System.Collections.Generic;
using System.Linq;
using ModernCashFlow.Domain.BaseInterfaces;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.Commands;
using ModernCashFlow.Excel2010.Forms;
using ModernCashFlow.WpfControls;
using Ninject;
using ModernCashFlow.Domain.Dtos;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    /// <summary>
    /// Responsible to coordinate the interaction between various parts of the program, like panels, ribbons, external services and worksheet data.
    /// </summary>
    public class CommandManager
    {

        [Inject]       
        public BaseController<Expense> ExpenseController { get; set; }

        [Inject]
        public BaseController<Income> IncomeController { get; set; }

       
        public void IncludeNewExpenseTransactions()
        {
            
            foreach (var expense in ExpenseController.CurrentSessionData.Where(e => e.IsTransient))
            {
                expense.EditStatus = expense.CanBeUsedInCashFlow ? EditStatus.Complete : EditStatus.Incomplete;
            }
            ExpenseController.RefreshAllLocalData();
        }

        public void IncludeNewIncomeTransactions()
        {
            foreach (var income in IncomeController.CurrentSessionData.Where(i => i.IsTransient))
            {
                income.EditStatus = income.CanBeUsedInCashFlow ? EditStatus.Complete : EditStatus.Incomplete;
            }
            ExpenseController.RefreshAllLocalData();
        }


        
       
    }
}