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
    public class ConfigWorksheet : WorksheetHelperBase<int>
    {
        private static BaseController<Account> _controller;
        private static CommandManager _commandManager;

        [Inject]
        public ConfigWorksheet(CommandManager commandManager, BaseController<Account> controller) : base(Globals.Config,Globals.Config.tblAccounts)
        {
            _commandManager = commandManager;
            _controller = controller;
            _controller.UpdateAllLocalData += OnUpdateAllLocalData;
            _controller.UpdateSingleLocalData += OnUpdateSingleLocalData;
            _controller.RetrieveAllLocalData += OnRetrieveLocalData;
        }

        private void OnUpdateSingleLocalData(Account updatedData)
        {
            Unprotect(enableEvents: false);

            var range = RowIndex[updatedData.Id];

            WriteWorksheetRow(range, updatedData);
         
            Protect();

        }

        private IEnumerable<Account> OnRetrieveLocalData()
        {
            return this.ReadFromWorksheet();
        }

        private IEnumerable<Account> ReadFromWorksheet()
        {
            var saidas = new List<Account>();

            try
            {
                object[,] dados = Table.Range.Value2;

                for (var row = 2; row <= dados.GetLength(0); row++)
                {
                    var entity = new Account();
                    ReadListObjectRow(row, dados, entity);

                    RowIndex.Set(entity.Id, (Range)Table.Range[row, Cols[Lang.TransactionCode]]);

                    saidas.Add(entity);
                }

                return saidas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }


        }

        private void OnUpdateAllLocalData(IEnumerable<Account> updatedData)
        {
            Unprotect(enableEvents: false);

            var data = updatedData.ToList();

            Table.SetDataBinding(data, "", DatabindCols);
            Table.Disconnect();

            Protect();
        }

        private static void ReadWorksheetRow(Range row, Account e)
        {
            var r = row.EntireRow;
            
            throw new NotImplementedException();
        }

        private static void WriteWorksheetRow(Range row, Account e)
        {
            
            //utilizando nomes menores de variável para facilitar leitura
            var r = row.EntireRow;


            
            throw new NotImplementedException();

        }

        private static void ReadListObjectRow(int row, object[,] dados, Account e)
        {
            throw new NotImplementedException();
            ;
        }
    }
}