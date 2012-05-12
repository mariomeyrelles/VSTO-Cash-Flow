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
    public class ConfigWorksheet
    {
        
        private static ListObject _tbl;
        private static readonly Config _sheet = Globals.Config;
        private static Dictionary<string, int> _cols;
        private static Dictionary<string, int> _absCols;
        private static Dictionary<int, Range> _index;
        private string[] _databindCols;
        private static BaseController<Account> _controller;
        private static CommandManager _commandManager;

        [Inject]
        public ConfigWorksheet(CommandManager commandManager, BaseController<Account> controller)
        {
            _commandManager = commandManager;
            _controller = controller;
            _controller.UpdateAllLocalData += WriteToWorksheet;
            _controller.UpdateSingleLocalData += OnUpdateSingleLocalData;
            _controller.RetrieveAllLocalData += OnRetrieveLocalData;
            _tbl = _sheet.tblAccounts;
            _index = new Dictionary<int, Range>();
        }

        public void ReadColumnPositions()
        {
            _cols = new Dictionary<string, int>();

            object[,] columnData = _tbl.HeaderRowRange.Value;

            //percorrer todas as colunas do array (e não linhas, pois sei que tem apenas 1 linha) e cadastrar as colunas no dicionário para uso geral.
            //o nome das colunas não pode ser alterado em hipótese nenhuma.
            for (var i = 1; i <= columnData.GetLength(1); i++)
            {
                _cols.Add(columnData[1, i].ToString(), i);
            }

            _absCols = new Dictionary<string, int>();

            var leftCol = _tbl.ListColumns.Item[1].Range.Column - 1;
            for (var i = 1; i <= columnData.GetLength(1); i++)
            {
                _absCols.Add(columnData[1, i].ToString(), leftCol + i);
            }

            _databindCols = ExcelUtil.PrepareColumnNamesForDatabinding<Account>(_cols.Keys.ToList());
        }

        private void OnUpdateSingleLocalData(Account updatedData)
        {
            Unprotect(enableEvents: false);

            var range = _index[updatedData.Id];

            WriteWorksheetRow(range, updatedData);
         
            Protect();

        }

        private IEnumerable<Account> OnRetrieveLocalData()
        {
            return this.ReadFromWorksheet();
        }

        public List<Account> ReadFromWorksheet()
        {
            var saidas = new List<Account>();

            try
            {
                object[,] dados = _tbl.Range.Value;

                for (var row = 2; row <= dados.GetLength(0); row++)
                {
                    var entity = new Account();
                    ReadListObjectRow(row, dados, entity);

                    _index.Set(entity.Id, (Range)_tbl.Range[row, _cols[MainResources.TransactionCode]]);

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

        private void WriteToWorksheet(IEnumerable<Account> updatedData)
        {
            Unprotect(enableEvents: false);

            var data = updatedData.ToList();

            _tbl.SetDataBinding(data, "", _databindCols);
            _tbl.Disconnect();

            Protect();
        }

        public static void ReadWorksheetRow(Range row, Account e)
        {
            var r = row.EntireRow;
            
            throw new NotImplementedException();
        }

        public static void WriteWorksheetRow(Range row, Account e)
        {
            //todo: validar tipos de dados e só escrever o que tiver mudado.
            //utilizando nomes menores de variável para facilitar leitura
            var r = row.EntireRow;
            
            throw new NotImplementedException();

        }

        public static void ReadListObjectRow(int row, object[,] dados, Account e)
        {
            
            throw new NotImplementedException();
            ;
        }

        /// <summary>
        /// Proteger o table contra modificações.
        /// </summary>
        public static void Protect()
        {
            _sheet.Protect(allowFormattingColumns: true, allowFormattingRows: true, allowSorting: true, allowFiltering: true, allowUsingPivotTables: true);
            Globals.ThisWorkbook.ThisApplication.EnableEvents = true;
        }

        /// <summary>
        /// Desproteger a planilha para modificações
        /// </summary>
        public static void Unprotect(bool enableEvents = true)
        {
            _sheet.Unprotect();
            if (!enableEvents)
            {
                Globals.ThisWorkbook.ThisApplication.EnableEvents = false;
            }

        }
    }
}