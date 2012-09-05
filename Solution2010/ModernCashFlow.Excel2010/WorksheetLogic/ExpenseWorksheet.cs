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
using ModernCashFlow.Excel2010.Commands;
using ModernCashFlow.Globalization.Resources;
using ModernCashFlow.Tools;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

using Ninject;
using Microsoft.Office.Interop.Excel;
using ListObject = Microsoft.Office.Tools.Excel.ListObject;

namespace ModernCashFlow.Excel2010.WorksheetLogic
{
    public class ExpenseWorksheet : WorksheetHelperBase<Guid,Income>
    {
        private static BaseController<Expense> _controller;
        private static CommandManager _commandManager;
        private BaseController<Account> _accountController;

        [Inject]
        public ExpenseWorksheet(CommandManager commandManager, BaseController<Expense> controller)
            : base(Globals.Expenses, Globals.Expenses.tblExpenses)
        {
            _commandManager = commandManager;
            _controller = controller;
            _controller.UpdateAllLocalData += OnUpdateAllLocalData;
            _controller.UpdateSingleLocalData += OnUpdateSingleLocalData;
            _controller.RetrieveAllLocalData += OnRetrieveLocalData;

            _accountController = NinjectContainer.Kernel.Get<BaseController<Account>>();
        }

        #region Worksheet Startup

        public void Start()
        {
            ReadColumnPositions();
            PrepareColumnsForDatabinding();
            ConfigureValidationLists();
        }

        private void ConfigureValidationLists()
        {
            Unprotect();

            var statuses = Util.GetEnumDescriptions(typeof(TransactionStatus));
            SetValidationForColumn(statuses, Lang.TransactionStatusDescription);

            var accountNames = _accountController.CurrentSessionData.Select(x => x.Name).ToList();
            SetValidationForColumn(accountNames, Lang.AccountName);

            Protect();
        }

       

        #endregion


        #region Event Handlers - Controller Events

        private IEnumerable<Expense> OnRetrieveLocalData()
        {
            return ReadFromWorksheet();
        }
        private void OnUpdateSingleLocalData(Expense updatedData)
        {
            Unprotect();

            var range = RowIndex[updatedData.TransactionCode];

            WriteWorksheetRow(range, updatedData);

            //todo: rever se precisa fazer isso em outros campos.
            Sheet.Range[string.Format("tblExpenses[{0}]", Lang.ExpectedValue)].NumberFormat = ExcelNumberFormats.Accounting;
            Sheet.Range[string.Format("tblExpenses[{0}]", Lang.ActualValue)].NumberFormat = ExcelNumberFormats.Accounting;

            Protect();

        }


        private void OnUpdateAllLocalData(IEnumerable<Expense> updatedData)
        {
            Unprotect();

            var data = updatedData.ToList();

            var databindingArray = new object[data.Count, Cols.Count];

            for (var i = 0; i < data.Count; i++)
            {
                databindingArray[i, Cols[Lang.TransactionDate] - 1] = data[i].TransactionDate_OA;
                databindingArray[i, Cols[Lang.Date] - 1] = data[i].Date_OA;
                databindingArray[i, Cols[Lang.ExpectedValue] - 1] = data[i].ExpectedValue;
                databindingArray[i, Cols[Lang.AccountName] - 1] = data[i].AccountName;
                databindingArray[i, Cols[Lang.Reason] - 1] = data[i].Reason;
                databindingArray[i, Cols[Lang.Place] - 1] = data[i].Place;
                databindingArray[i, Cols[Lang.ResponsibleName] - 1] = data[i].ResponsibleName;
                databindingArray[i, Cols[Lang.CategoryName] - 1] = data[i].CategoryName;
                databindingArray[i, Cols[Lang.Tags] - 1] = data[i].Tags;
                databindingArray[i, Cols[Lang.Quantity] - 1] = data[i].Quantity;
                databindingArray[i, Cols[Lang.ActualValue] - 1] = data[i].ActualValue;
                databindingArray[i, Cols[Lang.TransactionStatusDescription] - 1] = data[i].TransactionStatusDescription;
                databindingArray[i, Cols[Lang.EditStatus] - 1] = data[i].EditStatus.ToString();
                databindingArray[i, Cols[Lang.CheckNumber] - 1] = data[i].CheckNumber;
                databindingArray[i, Cols[Lang.TransactionCode] - 1] = data[i].TransactionCode.ToString();
                databindingArray[i, Cols[Lang.Remarks] - 1] = data[i].Remarks;
                
            }

            Table.Resize(Table.Range.Resize[data.Count + 1]);
            Table.DataBodyRange.Value2 = databindingArray;
            Table.Range.Columns.AutoFit();

            Sheet.Range[string.Format("tblExpenses[{0}]", Lang.ExpectedValue)].NumberFormat = ExcelNumberFormats.Accounting;
            Sheet.Range[string.Format("tblExpenses[{0}]", Lang.ActualValue)].NumberFormat = ExcelNumberFormats.Accounting;

            Protect();
        }

        #endregion


        #region Worksheet I/O

        private IEnumerable<Expense> ReadFromWorksheet()
        {
            var saidas = new List<Expense>();

            try
            {
                object[,] dados = Table.Range.Value2;

                for (var row = 2; row <= dados.GetLength(0); row++)
                {
                    var saida = new Expense();
                    ReadListObjectRow(row, dados, saida);

                    RowIndex.Set(saida.TransactionCode, (Range)Table.Range[row, Cols[Lang.TransactionCode]]);

                    saidas.Add(saida);
                }

                return saidas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        private void ReadWorksheetRow(Range row, Expense s)
        {
            var r = row.EntireRow;
            s.TransactionDate = RangeUtils.ToDateTime(r.Cells[1, AbsCols[Lang.TransactionDate]]);
            s.Date = RangeUtils.ToDateTime(r.Cells[1, AbsCols[Lang.Date]]);
            s.ExpectedValue = RangeUtils.ToDecimal(r.Cells[1, AbsCols[Lang.ExpectedValue]]);
            s.AccountName = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.AccountName]]);
            s.Reason = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Reason]]);
            s.Place = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Place]]);
            s.ResponsibleName = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.ResponsibleName]]);
            s.CategoryName = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.CategoryName]]);
            s.Tags = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Tags]]);
            s.Quantity = RangeUtils.ToDecimal(r.Cells[1, AbsCols[Lang.Quantity]]);
            s.ActualValue = RangeUtils.ToDecimal(r.Cells[1, AbsCols[Lang.ActualValue]]);
            s.TransactionStatus = EnumTools.GetValueFromDescription<TransactionStatus>(RangeUtils.ToString(r.Cells[1, AbsCols[Lang.TransactionStatusDescription]]));
            s.EditStatus = EnumTools.GetValueFromDescription<EditStatus>(RangeUtils.ToString(r.Cells[1, AbsCols[Lang.EditStatus]]));
            s.CheckNumber = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.CheckNumber]]);
            s.TransactionCode = RangeUtils.ToGuid(r.Cells[1, AbsCols[Lang.TransactionCode]]) ?? Guid.NewGuid();
            s.Remarks = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Remarks]]);
        }

        private void ReadListObjectRow(int row, object[,] dados, Expense s)
        {
            s.TransactionDate = Parse.ToDateTime(dados[row, Cols[Lang.TransactionDate]]) ?? DateTime.Now;
            s.Date = Parse.ToDateTime(dados[row, Cols[Lang.Date]]);
            s.ExpectedValue = Parse.ToDecimal(dados[row, Cols[Lang.ExpectedValue]]);
            s.AccountName = Parse.ToString(dados[row, Cols[Lang.AccountName]]);
            s.Reason = Parse.ToString(dados[row, Cols[Lang.Reason]]);
            s.Place = Parse.ToString(dados[row, Cols[Lang.Place]]);
            s.ResponsibleName = Parse.ToString(dados[row, Cols[Lang.ResponsibleName]]);
            s.CategoryName = Parse.ToString(dados[row, Cols[Lang.CategoryName]]);
            s.Tags = Parse.ToString(dados[row, Cols[Lang.Tags]]);
            s.Quantity = Parse.ToDecimal(dados[row, Cols[Lang.Quantity]]);
            s.ActualValue = Parse.ToDecimal(dados[row, Cols[Lang.ActualValue]]);
            s.TransactionStatus = EnumTools.GetValueFromDescription<TransactionStatus>(Parse.ToString(dados[row, Cols[Lang.TransactionStatusDescription]]));
            s.EditStatus = EnumTools.GetValueFromDescription<EditStatus>(Parse.ToString(dados[row, Cols[Lang.EditStatus]]));
            s.CheckNumber = Parse.ToString(dados[row, Cols[Lang.CheckNumber]]);
            s.TransactionCode = Parse.ToGuid(dados[row, Cols[Lang.TransactionCode]]) ?? Guid.NewGuid();
            s.Remarks = Parse.ToString(dados[row, Cols[Lang.Remarks]]);
        }

        private void WriteWorksheetRow(Range row, Expense s)
        {
            var r = row.EntireRow;
            r.Cells[1, AbsCols[Lang.TransactionDate]].Value2 = s.TransactionDate ?? r.Cells[1, AbsCols[Lang.TransactionDate]].Value2;
            r.Cells[1, AbsCols[Lang.Date]].Value2 = s.Date ?? r.Cells[1, AbsCols[Lang.Date]].Value2;
            r.Cells[1, AbsCols[Lang.ExpectedValue]].Value2 = s.ExpectedValue ?? r.Cells[1, AbsCols[Lang.ExpectedValue]].Value2;
            r.Cells[1, AbsCols[Lang.AccountName]].Value2 = s.AccountName ?? r.Cells[1, AbsCols[Lang.AccountName]].Value2;
            r.Cells[1, AbsCols[Lang.Reason]].Value2 = s.Reason ?? r.Cells[1, AbsCols[Lang.Reason]].Value2;
            r.Cells[1, AbsCols[Lang.Place]].Value2 = s.Place ?? r.Cells[1, AbsCols[Lang.Place]].Value2;
            r.Cells[1, AbsCols[Lang.ResponsibleName]].Value2 = s.ResponsibleName ?? r.Cells[1, AbsCols[Lang.ResponsibleName]].Value2;
            r.Cells[1, AbsCols[Lang.CategoryName]].Value2 = s.CategoryName ?? r.Cells[1, AbsCols[Lang.CategoryName]].Value2;
            r.Cells[1, AbsCols[Lang.Tags]].Value2 = s.Tags ?? r.Cells[1, AbsCols[Lang.Tags]].Value2;
            r.Cells[1, AbsCols[Lang.Quantity]].Value2 = s.Quantity ?? r.Cells[1, AbsCols[Lang.Quantity]].Value2;
            r.Cells[1, AbsCols[Lang.ActualValue]].Value2 = s.ActualValue ?? r.Cells[1, AbsCols[Lang.ActualValue]].Value2;
            r.Cells[1, AbsCols[Lang.TransactionStatusDescription]].Value2 = s.TransactionStatusDescription ?? r.Cells[1, AbsCols[Lang.TransactionStatusDescription]].Value2;
            r.Cells[1, AbsCols[Lang.EditStatus]].Value2 = s.EditStatus.ToString();
            r.Cells[1, AbsCols[Lang.CheckNumber]].Value2 = s.CheckNumber ?? r.Cells[1, AbsCols[Lang.CheckNumber]].Value2;
            r.Cells[1, AbsCols[Lang.TransactionCode]].Value2 = s.TransactionCode.ToString();
            r.Cells[1, AbsCols[Lang.Remarks]].Value2 = s.Remarks ?? r.Cells[1, AbsCols[Lang.Remarks]].Value2;
        }

        #endregion


        #region Nested classes for ListObject functionality

        public class Events
        {
            private Excel.Range _activeRange;
            private readonly ExpenseWorksheet _parent;
            
            [Inject]
            public Events(ExpenseWorksheet parent)
            {
                _parent = parent;
            }
            

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


                    var columnIndex = _parent.AbsCols[Lang.TransactionCode];

                    Guid codLancamento = RangeUtils.ToGuid(targetRange.EntireRow.Cells[1, columnIndex]);
                    var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                    _parent.ReadWorksheetRow(targetRange, entity);

                    _controller.AcceptData(entity, true);

                    CommandHandler.Send<UpdateSidePanelCommand>(new SidePanelCommandArgs { Model = entity });
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

                    var codLancamento = RangeUtils.ToGuid(_activeRange.EntireRow.Cells[1, _parent.AbsCols[Lang.TransactionCode]]);
                    if (codLancamento == null)
                        return;

                    var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                    CommandHandler.Send<UpdateSidePanelCommand>(new SidePanelCommandArgs { Model = entity });
                }
                catch (Exception)
                {
                    //não fazer nada pois selecionar outros tipos de campo do listobject, como headers e footers não me afeta em nada.

                }
            }

            public bool CanSaveLocalData()
            {
                return _controller.CurrentSessionData.All(x => x.EditStatus != EditStatus.Created);
            }

            public void BeforeSave(bool saveAsUi, ref bool cancel)
            {
                if (!CanSaveLocalData())
                {
                    var result = MessageBox.Show("Foram criados novos lançamentos. Deseja incluí-los no fluxo de caixa agora?",
                                    "Inclusão de novos lançamentos", MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK)
                    {
                        _commandManager.IncludeNewExpenseTransactions();
                    }
                }
            }
        }

        public class ContextMenus
        {
            private Office.CommandBar _commandBar;
            //private Office.CommandBarButton _menuEdit;
           // private Office.CommandBarButton _menuSalvar;
            private Office.CommandBarButton _menuRemover;
            private Office.CommandBarButton _menuInserir;
            private Excel.Range _activeRange;
            private readonly ExpenseWorksheet _parent;

            [Inject]
            public ContextMenus(ExpenseWorksheet parent)
            {
                _parent = parent;
                this.Prepare();

            }

            private void Prepare()
            {
                //fonte dos ícones (faceid): http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/

                //criar um novo command bar do tipo popup para acomodar os itens criados abaixo.
                _commandBar = Globals.ThisWorkbook.Application.CommandBars.Add("ExpenseContextMenu", Office.MsoBarPosition.msoBarPopup, false, true);

                _menuInserir = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                _menuInserir.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                _menuInserir.Caption = "New Expense...";
                _menuInserir.FaceId = 1544;
                _menuInserir.Tag = "4a";

                //_menuEdit = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                //_menuEdit.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                //_menuEdit.Caption = "Editar Item...";
                //_menuEdit.FaceId = 0162;
                //_menuEdit.Tag = "0";

                //_menuSalvar = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                //_menuSalvar.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                //_menuSalvar.Caption = "Salvar...";
                //_menuSalvar.FaceId = 1975;
                //_menuSalvar.Tag = "2";

                _menuRemover = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                _menuRemover.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                _menuRemover.Caption = "Remove Expense...";
                _menuRemover.FaceId = 0478;
                _menuRemover.Tag = "3a";


                //_menuEdit.Click += MenuEditClick;
                //_menuSalvar.Click += MenuSaveClick;
                //_menuRemover.Click += MenuRemoveClick;
                _menuInserir.Click += this.MenuCreateClick;
            }

            private void MenuCreateClick(CommandBarButton ctrl, ref bool canceldefault)
            {
                _parent.Unprotect();

                //criar uma nova entidade Saída.
                var newExpense = new Expense();
                newExpense.Date = DateTime.Now;
                newExpense.TransactionDate = DateTime.Now;
                newExpense.TransactionCode = Guid.NewGuid();
                newExpense.EditStatus = EditStatus.Created;
                newExpense.TransactionStatus = TransactionStatus.Unknown;

                //solicitar ao controller que aceite os novos dados.
                _controller.AcceptData(newExpense);

                //configurar a linha nova da planilha com valores default.
                var newRow = _parent.Table.ListRows.Add();
                newRow.Range[1, _parent.Cols[Lang.Date]].Value2 = newExpense.Date;
                newRow.Range[1, _parent.Cols[Lang.TransactionDate]].Value2 = newExpense.TransactionDate;
                newRow.Range[1, _parent.Cols[Lang.TransactionCode]].Value2 = newExpense.TransactionCode.ToString();
                newRow.Range[1, _parent.Cols[Lang.EditStatus]].Value2 = newExpense.TransactionStatusDescription;
                newRow.Range[1, _parent.Cols[Lang.TransactionStatusDescription]].Value2 = newExpense.TransactionStatus.GetDescription();

                //atualizar o índice de linhas com esta nova saída.
                _parent.RowIndex.Set(newExpense.TransactionCode, (Range)newRow.Range[1, _parent.Cols[Lang.TransactionCode]]);

                newRow.Range[1, 1].Select();

                _parent.Protect();
            }

            private void MenuEditClick(CommandBarButton ctrl, ref bool canceldefault)
            {
                Guid codLancamento = RangeUtils.ToGuid(_activeRange.EntireRow.Cells[1, _parent.AbsCols[Lang.TransactionCode]]);

                var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                CommandHandler.Send<UpdateSidePanelCommand>(new SidePanelCommandArgs { Model = entity });

            }


            public void ShowContextMenu(Excel.Range target, ref bool cancel)
            {
                _activeRange = target;
                // _commandBar.ShowPopup();
                Globals.ThisWorkbook.Application.CommandBars["ExpenseContextMenu"].ShowPopup();
                cancel = true;
            }
        }

        #endregion
    }


}