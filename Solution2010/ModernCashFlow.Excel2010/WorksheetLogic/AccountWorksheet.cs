using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Globalization.Resources;
using ModernCashFlow.Tools;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

using Ninject;
using Microsoft.Office.Interop.Excel;
using ListObject = Microsoft.Office.Tools.Excel.ListObject;

namespace ModernCashFlow.Excel2010.WorksheetLogic
{
    public class AccountWorksheet : WorksheetHelperBase<int,Account>
    {
        private static BaseController<Account> _controller;
        private static CommandManager _commandManager;

        [Inject]
        public AccountWorksheet(CommandManager commandManager, BaseController<Account> controller) : base(Globals.Config,Globals.Config.tblAccounts)
        {
            _commandManager = commandManager;
            _controller = controller;
            _controller.UpdateAllLocalData += OnUpdateAllLocalData;
            _controller.UpdateSingleLocalData += OnUpdateSingleLocalData;
            _controller.RetrieveAllLocalData += OnRetrieveLocalData;
        }

        public void Start()
        {
            ReadColumnPositions();
        }


        #region Event Handlers - Controller Events

        private void OnUpdateAllLocalData(IEnumerable<Account> updatedData)
        {
            Unprotect();

            var data = updatedData.ToList();

            Table.SetDataBinding(data, "", DatabindCols);
            Table.Disconnect();

            Protect();
        }

        private IEnumerable<Account> OnRetrieveLocalData()
        {
            return ReadFromWorksheet();
        }
        private void OnUpdateSingleLocalData(Account updatedData)
        {
            Unprotect();

            var range = RowIndex[updatedData.Id];

            WriteWorksheetRow(range, updatedData);

            Protect();

        }


        #endregion



        #region Worksheet I/O


        private IEnumerable<Account> ReadFromWorksheet()
        {
            var accounts = new List<Account>();

            try
            {
                object[,] data = Table.Range.Value2;

                for (var row = 2; row <= data.GetLength(0); row++)
                {
                    var entity = new Account();
                    ReadListObjectRow(row, data, entity);

                    RowIndex.Set(entity.Id, (Range)Table.Range[row, Cols["Id"]]);

                    accounts.Add(entity);
                }

                return accounts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }


        }
        

        private void ReadWorksheetRow(Range row, Account a)
        {
            throw new NotImplementedException();
        }

        private void WriteWorksheetRow(Range row, Account a)
        {
            
            //utilizando nomes menores de variável para facilitar leitura
            var r = row.EntireRow;


            
            throw new NotImplementedException();

        }

        private void ReadListObjectRow(int row, object[,] dados, Account a)
        {

            a.Id = Convert.ToInt32(dados[row, Cols["Id"]]);
            a.Name = Parse.ToString(dados[row, Cols["Name"]]);
            a.Description = Parse.ToString(dados[row, Cols["Description"]]);
            a.ResponsibleName = Parse.ToString(dados[row, Cols["ResponsibleName"]]);
            a.InitialBalance = Parse.ToDecimal(dados[row, Cols["InitialBalance"]]);
            a.InitialDate = Parse.ToDateTime(dados[row, Cols["InitialDate"]]); 
            a.AcceptsDeposits = Convert.ToBoolean(dados[row, Cols["AcceptsDeposits"]]);
            a.AcceptsManualAdjustment = Convert.ToBoolean(dados[row, Cols["AcceptsManualAdjustment"]]);
            a.AcceptsNegativeValues = Convert.ToBoolean(dados[row, Cols["AcceptsNegativeValues"]]);
            a.RequiresPayment = Convert.ToBoolean(dados[row, Cols["RequiresPayment"]]);
            a.AcceptsPartialPayment = Convert.ToBoolean(dados[row, Cols["AcceptsPartialPayment"]]);
            a.AcceptsLatePaymentInterest = Convert.ToBoolean(dados[row, Cols["AcceptsLatePaymentInterest"]]);
            a.AcceptsYield = Convert.ToBoolean(dados[row, Cols["AcceptsYield"]]);
            a.AcceptsChecks = Convert.ToBoolean(dados[row, Cols["AcceptsChecks"]]);
            a.CloseDay = Parse.ToInt(dados[row, Cols["CloseDay"]]);
            a.PaymentDay = Parse.ToInt(dados[row, Cols["PaymentDay"]]);
            a.MonthlyCost = Parse.ToDecimal(dados[row, Cols["MonthlyCost"]]);
        }

        #endregion
    }
}