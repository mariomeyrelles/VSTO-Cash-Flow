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
    public class IncomeWorksheet
    {
        private static ListObject _tbl;
        private static readonly Incomes _sheet = Globals.Incomes;
        private static Dictionary<string, int> _cols;
        private static Dictionary<string, int> _absCols;
        private static Dictionary<Guid, Range> _index;
        private string[] _databindCols;
        private static BaseController<Income> _controller;
        private static CommandManager _commandManager;

        [Inject]
        public IncomeWorksheet(CommandManager commandManager, BaseController<Income> controller)
        {
            _commandManager = commandManager;
            _controller = controller;
            _controller.UpdateAllLocalData += WriteToWorksheet;
            _controller.UpdateSingleLocalData += OnUpdateSingleLocalData;
            _controller.RetrieveAllLocalData += OnRetrieveLocalData;
            _tbl = _sheet.tblIncomes;
            _index = new Dictionary<Guid, Range>();
        }


        public void ConfigureValidationLists()
        {
            Unprotect();

            //todo: organizar melhor o código para ficar mais clara a montagem as listas de validação.
            var itens = Util.GetEnumDescriptions(typeof(TransactionStatus));

            var separator = Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
            var valores = string.Join(separator, itens);
            var range = _sheet.Range["tblIncomes[Status do Lançamento]"];
            range.Validation.Delete();
            range.Validation.Add(XlDVType.xlValidateList,
                XlDVAlertStyle.xlValidAlertInformation, XlFormatConditionOperator.xlBetween, valores);

            range.Validation.InCellDropdown = true;
            range.Validation.IgnoreBlank = true;

            Protect();
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

            _databindCols = ExcelUtil.PrepareColumnNamesForDatabinding<Income>(_cols.Keys.ToList());
        }



        private void OnUpdateSingleLocalData(Income updatedData)
        {
            Unprotect(enableEvents: false);

            var range = _index[updatedData.TransactionCode];

            WriteWorksheetRow(range, updatedData);

            _sheet.Range[string.Format("tblIncomes[{0}]", MainResources.ExpectedValue)].NumberFormat = ExcelNumberFormats.Accounting;
            _sheet.Range[string.Format("tblIncomes[{0}]", MainResources.ActualValue)].NumberFormat = ExcelNumberFormats.Accounting;
 
            Protect();

        }

        private IEnumerable<Income> OnRetrieveLocalData()
        {
            return this.ReadFromWorksheet();
        }

        private void WriteToWorksheet(IEnumerable<Income> updatedData)
        {
            Unprotect(enableEvents: false);

            var data = updatedData.ToList();

            _tbl.SetDataBinding(data, "", _databindCols);
            _tbl.Disconnect();

            //todo: manter a formatação dos demais campos para evitar que o usuário estrague a formatação do campo
            _sheet.Range[string.Format("tblIncomes[{0}]", MainResources.ExpectedValue)].NumberFormat = ExcelNumberFormats.Accounting;
            _sheet.Range[string.Format("tblIncomes[{0}]", MainResources.ActualValue)].NumberFormat = ExcelNumberFormats.Accounting;
            
            _tbl.Range.Columns.AutoFit();

            Protect();
        }

        /// <summary>
        /// Ler todos os dados da planilha e colocar na memória.
        /// </summary>
        /// <returns></returns>
        public List<Income> ReadFromWorksheet()
        {
            var saidas = new List<Income>();

            try
            {
                object[,] dados = _tbl.Range.Value;

                for (var row = 2; row <= dados.GetLength(0); row++)
                {
                    var entity = new Income();
                    ReadListObjectRow(row, dados, entity);

                    _index.Set(entity.TransactionCode, (Range)_tbl.Range[row, _cols[MainResources.TransactionCode]]);

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

        public static void ReadWorksheetRow(Range row, Income e)
        {
            var r = row.EntireRow;
            e.TransactionDate = RangeUtils.ToDateTime(r.Cells[1, _absCols[MainResources.TransactionDate]]);
            e.Date = RangeUtils.ToDateTime(r.Cells[1, _absCols[MainResources.Date]]);
            e.ExpectedValue = RangeUtils.ToDouble(r.Cells[1, _absCols[MainResources.ExpectedValue]]);
            e.AccountName = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.AccountName]]);
            e.Reason = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.Reason]]);
            e.Place = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.Place]]);
            e.ResponsibleName = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.ResponsibleName]]);
            e.CategoryName = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.CategoryName]]);
            e.Tags = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.Tags]]);
            e.Quantity = RangeUtils.ToDecimal(r.Cells[1, _absCols[MainResources.Quantity]]);
            e.ActualValue = RangeUtils.ToDouble(r.Cells[1, _absCols[MainResources.ActualValue]]);
            e.TransactionStatus = EnumTools.GetValueFromDescription<TransactionStatus>(RangeUtils.ToString(r.Cells[1, _absCols[MainResources.TransactionStatusDescription]]));
            e.EditStatus = EnumTools.GetValueFromDescription<EditStatus>(RangeUtils.ToString(r.Cells[1, _absCols[MainResources.EditStatus]]));
            e.DueDate = RangeUtils.ToDateTime(r.Cells[1, _absCols[MainResources.DueDate]]);
            e.IsRecurring = RangeUtils.ToBoolean(r.Cells[1, _absCols[MainResources.IsRecurring]]);
            e.MonthlyInterval = RangeUtils.ToInt(r.Cells[1, _absCols[MainResources.MonthlyInterval]]);
            e.RemainingInstallments = RangeUtils.ToInt(r.Cells[1, _absCols[MainResources.RemainingInstallments]]);
            e.AccountTransferCode = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.AccountTransferCode]]);
            e.CheckNumber = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.CheckNumber]]);
            e.SupportsDrillDown = RangeUtils.ToBoolean(r.Cells[1, _absCols[MainResources.SupportsDrillDown]]);
            e.TransactionGroup = RangeUtils.ToGuid(r.Cells[1, _absCols[MainResources.TransactionGroup]]);
            e.TransactionCode = RangeUtils.ToGuid(r.Cells[1, _absCols[MainResources.TransactionCode]]) ?? Guid.NewGuid();
            e.Remarks = RangeUtils.ToString(r.Cells[1, _absCols[MainResources.Remarks]]);
        }

        public static void WriteWorksheetRow(Range row, Income e)
        {
            //todo: validar tipos de dados e só escrever o que tiver mudado.
            //utilizando nomes menores de variável para facilitar leitura
            var r = row.EntireRow;
            r.Cells[1, _absCols[MainResources.TransactionDate]].Value2 = e.TransactionDate ?? r.Cells[1, _absCols[MainResources.TransactionDate]].Value2;
            r.Cells[1, _absCols[MainResources.Date]].Value2 = e.Date ?? r.Cells[1, _absCols[MainResources.Date]].Value2;
            r.Cells[1, _absCols[MainResources.ExpectedValue]].Value2 = e.ExpectedValue ?? r.Cells[1, _absCols[MainResources.ExpectedValue]].Value2;
            r.Cells[1, _absCols[MainResources.AccountName]].Value = e.AccountName ?? r.Cells[1, _absCols[MainResources.AccountName]].Value;
            r.Cells[1, _absCols[MainResources.Reason]].Value = e.Reason ?? r.Cells[1, _absCols[MainResources.Reason]].Value;
            r.Cells[1, _absCols[MainResources.Place]].Value = e.Place ?? r.Cells[1, _absCols[MainResources.Place]].Value;
            r.Cells[1, _absCols[MainResources.ResponsibleName]].Value = e.ResponsibleName ?? r.Cells[1, _absCols[MainResources.ResponsibleName]].Value;
            r.Cells[1, _absCols[MainResources.CategoryName]].Value = e.CategoryName ?? r.Cells[1, _absCols[MainResources.CategoryName]].Value;
            r.Cells[1, _absCols[MainResources.Tags]].Value = e.Tags ?? r.Cells[1, _absCols[MainResources.Tags]].Value;
            r.Cells[1, _absCols[MainResources.Quantity]].Value = e.Quantity ?? r.Cells[1, _absCols[MainResources.Quantity]].Value;
            r.Cells[1, _absCols[MainResources.ActualValue]].Value2 = e.ActualValue ?? r.Cells[1, _absCols[MainResources.ActualValue]].Value2;
            r.Cells[1, _absCols[MainResources.TransactionStatusDescription]].Value = e.TransactionStatusDescription ?? r.Cells[1, _absCols[MainResources.TransactionStatusDescription]].Value;
            r.Cells[1, _absCols[MainResources.EditStatus]].Value = e.EditStatus.ToString();
            r.Cells[1, _absCols[MainResources.DueDate]].Value2 = e.DueDate ?? r.Cells[1, _absCols[MainResources.DueDate]].Value2;
            r.Cells[1, _absCols[MainResources.IsRecurring]].Value = e.IsRecurring ?? r.Cells[1, _absCols[MainResources.IsRecurring]].Value;
            r.Cells[1, _absCols[MainResources.MonthlyInterval]].Value = e.MonthlyInterval ?? r.Cells[1, _absCols[MainResources.MonthlyInterval]].Value;
            r.Cells[1, _absCols[MainResources.RemainingInstallments]].Value = e.RemainingInstallments ?? r.Cells[1, _absCols[MainResources.RemainingInstallments]].Value;
            r.Cells[1, _absCols[MainResources.AccountTransferCode]].Value = e.AccountTransferCode ?? r.Cells[1, _absCols[MainResources.AccountTransferCode]].Value;
            r.Cells[1, _absCols[MainResources.CheckNumber]].Value = e.CheckNumber ?? r.Cells[1, _absCols[MainResources.CheckNumber]].Value;
            r.Cells[1, _absCols[MainResources.SupportsDrillDown]].Value = e.SupportsDrillDown ?? r.Cells[1, _absCols[MainResources.SupportsDrillDown]].Value;
            r.Cells[1, _absCols[MainResources.TransactionGroup]].Value = e.TransactionGroup ?? r.Cells[1, _absCols[MainResources.TransactionGroup]].Value;
            r.Cells[1, _absCols[MainResources.TransactionCode]].Value = e.TransactionCode.ToString();
            r.Cells[1, _absCols[MainResources.Remarks]].Value = e.Remarks ?? r.Cells[1, _absCols[MainResources.Remarks]].Value;


        }

        public static void ReadListObjectRow(int row, object[,] dados, Income e)
        {
            e.TransactionDate = Parse.ToDateTime(dados[row, _cols[MainResources.TransactionDate]]) ?? DateTime.Now;
            e.Date = Convert.ToDateTime(dados[row, _cols[MainResources.Date]]);
            e.ExpectedValue = Parse.ToDouble(dados[row, _cols[MainResources.ExpectedValue]]);
            e.AccountName = Parse.ToString(dados[row, _cols[MainResources.AccountName]]);
            e.Reason = Parse.ToString(dados[row, _cols[MainResources.Reason]]);
            e.Place = Parse.ToString(dados[row, _cols[MainResources.Place]]);
            e.ResponsibleName = Parse.ToString(dados[row, _cols[MainResources.ResponsibleName]]);
            e.CategoryName = Parse.ToString(dados[row, _cols[MainResources.CategoryName]]);
            e.Tags = Parse.ToString(dados[row, _cols[MainResources.Tags]]);
            e.Quantity = Parse.ToDecimal(dados[row, _cols[MainResources.Quantity]]);
            e.ActualValue = Parse.ToDouble(dados[row, _cols[MainResources.ActualValue]]);
            e.TransactionStatus = EnumTools.GetValueFromDescription<TransactionStatus>(Parse.ToString(dados[row, _cols[MainResources.TransactionStatusDescription]]));
            e.EditStatus = EnumTools.GetValueFromDescription<EditStatus>(Parse.ToString(dados[row, _cols[MainResources.EditStatus]]));
            e.DueDate = Parse.ToDateTime(dados[row, _cols[MainResources.DueDate]]);
            e.IsRecurring = Parse.ToBoolean(dados[row, _cols[MainResources.IsRecurring]]);
            e.MonthlyInterval = Parse.ToInt(dados[row, _cols[MainResources.MonthlyInterval]]);
            e.RemainingInstallments = Parse.ToInt(dados[row, _cols[MainResources.RemainingInstallments]]);
            e.AccountTransferCode = Parse.ToString(dados[row, _cols[MainResources.AccountTransferCode]]);
            e.CheckNumber = Parse.ToString(dados[row, _cols[MainResources.CheckNumber]]);
            e.SupportsDrillDown = Parse.ToBoolean(dados[row, _cols[MainResources.SupportsDrillDown]]);
            e.TransactionGroup = Parse.ToGuid(dados[row, _cols[MainResources.TransactionGroup]]);
            e.TransactionCode = Parse.ToGuid(dados[row, _cols[MainResources.TransactionCode]]) ?? Guid.NewGuid();
            e.Remarks = Parse.ToString(dados[row, _cols[MainResources.Remarks]]);
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



        public class Events
        {
            private Excel.Range _activeRange;

            public void OnChange(Excel.Range targetRange, ListRanges changedEvents)
            {
                //Debug.WriteLine(targetRange.Address + "; Células:  " + targetRange.Cells.Count + "; Linhas: " + targetRange.Rows.Count);
                try
                {
                    //Para simplificar o tratamento das alterações não é permitido alterar mais de uma célula por vez.
                    if (targetRange.Cells.Count != 1)
                    {
                        //caso o usuário tente isso, este código executa uma função de Undo e não faz mais nada. 
                        Globals.ThisWorkbook.Application.Undo();
                        return;
                    }

                    Guid codLancamento = RangeUtils.ToGuid(targetRange.EntireRow.Cells[1, _absCols[MainResources.TransactionCode]]);
                    var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                    ReadWorksheetRow(targetRange, entity);

                    _controller.AcceptData(entity, true);
                    _commandManager.UpdateSidePanel(entity);
                }
                catch (Exception ex)
                {
                    //todo: decidir o que fazer ao dar erro do change
                    MessageBox.Show(ex.ToString());
                }
            }

            public void OnSelectionChange(Range target)
            {
                try
                {
                    if (target.Cells.Count != 1)
                        return;

                    _activeRange = target;

                    var codLancamento = RangeUtils.ToGuid(_activeRange.EntireRow.Cells[1, _absCols[MainResources.TransactionCode]]);
                    if (codLancamento == null)
                    {
                        MessageBox.Show(MainResources.NullTransactionCode);
                        return;
                    }

                    var entity = _controller.CurrentSessionData.Where(x => x.TransactionCode == codLancamento).FirstOrDefault();
                    _commandManager.UpdateSidePanel(entity);
                }
                catch (Exception)
                {
                    //não fazer nada pois selecionar outros tipos de campo do listobject, como headers e footers não me afeta em nada.

                }
            }

            public bool CanSaveLocalData()
            {
                return !_controller.CurrentSessionData.Any(x => x.EditStatus == EditStatus.Created);
            }

            public void BeforeSave(bool saveAsUi, ref bool cancel)
            {
                //check if this is really necessary
                if (!CanSaveLocalData())
                {
                    var result = MessageBox.Show("Foram criados novos lançamentos. Deseja incluí-los no fluxo de caixa agora?",
                                    "Inclusão de novos lançamentos", MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK)
                    {
                        _commandManager.IncludeNewIncomeTransactions();
                    }
                }
            }
        }

        public class ContextMenus
        {
            private CommandBar _commandBar;
            private CommandBarButton _menuEdit;
            private CommandBarButton _menuSalvar;
            private CommandBarButton _menuRemover;
            private CommandBarButton _menuInserir;
            private Range _activeRange;

            public ContextMenus()
            {
                this.Prepare();
            }

            /// <summary>
            /// Montar quatro opções no menu de contexto, com seus devidos ícones e event handlers associados.
            /// </summary>
            private void Prepare()
            {
                //fonte dos ícones (faceid): http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/

                //criar um novo command bar do tipo popup para acomodar os itens criados abaixo.
                _commandBar = Globals.ThisWorkbook.Application.CommandBars.Add("ExpenseContextMenu", Office.MsoBarPosition.msoBarPopup, false, true);

                _menuInserir = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                _menuInserir.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                _menuInserir.Caption = "Add New Transaction...";
                _menuInserir.FaceId = 1544;
                _menuInserir.Tag = "4";

                //_menuEdit = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                //_menuEdit.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                //_menuEdit.Caption = "Editar Item...";
                //_menuEdit.FaceId = 0162;
                //_menuEdit.Tag = "0";

                _menuSalvar = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                _menuSalvar.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                _menuSalvar.Caption = "Salvar...";
                _menuSalvar.FaceId = 1975;
                _menuSalvar.Tag = "2";

                _menuRemover = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                _menuRemover.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                _menuRemover.Caption = "Remover Item...";
                _menuRemover.FaceId = 0478;
                _menuRemover.Tag = "3";


                //_menuEdit.Click += MenuEditClick;
                //_menuSalvar.Click += MenuSaveClick;
                //_menuRemover.Click += MenuRemoveClick;
                _menuInserir.Click += MenuCreateClick;
            }

            private void MenuCreateClick(CommandBarButton ctrl, ref bool canceldefault)
            {
                Unprotect();

                //criar uma nova entidade Saída.
                var e = new Income();
                e.TransactionDate = DateTime.Now;
                e.TransactionCode = Guid.NewGuid();
                e.EditStatus = EditStatus.Created;
                e.TransactionStatus = TransactionStatus.Unknown;

                //solicitar ao controller que aceite os novos dados.
                var controller = NinjectContainer.Kernel.Get<BaseController<Income>>();
                controller.AcceptData(e);

                //configurar a linha nova da planilha com valores default.
                var newRow = _tbl.ListRows.Add();
                newRow.Range[1, _cols[MainResources.TransactionDate]].Value = e.TransactionDate;
                newRow.Range[1, _cols[MainResources.TransactionCode]].Value = e.TransactionCode.ToString();
                newRow.Range[1, _cols[MainResources.EditStatus]].Value = e.TransactionStatusDescription;
                newRow.Range[1, _cols[MainResources.TransactionStatusDescription]].Value = e.TransactionStatus.GetDescription();

                //atualizar o índice de linhas com esta nova saída.
                _index.Set(e.TransactionCode, (Range)newRow.Range[1, _cols[MainResources.TransactionCode]]);

                Protect();
            }


            private void MenuEditClick(CommandBarButton ctrl, ref bool canceldefault)
            {
                Guid codLancamento = RangeUtils.ToGuid(_activeRange.EntireRow.Cells[1, _absCols[MainResources.TransactionCode]]);

                
                var entity = _controller.CurrentSessionData.Where(x => x.TransactionCode == codLancamento).FirstOrDefault();
                _commandManager.UpdateSidePanel(entity);

            }


            public void ShowContextMenu(Excel.Range target, ref bool cancel)
            {
                _activeRange = target;
                _commandBar.ShowPopup();
                cancel = true;
            }
        }

    }


}