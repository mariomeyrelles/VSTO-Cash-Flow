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
    public class IncomeWorksheet : WorksheetHelperBase<Guid, Income>
    {
        private static BaseController<Income> _controller;
        private static CommandManager _commandManager;

        private BaseController<Account> _accountController;

        [Inject]
        public IncomeWorksheet(CommandManager commandManager, BaseController<Income> controller) : base(Globals.Incomes,Globals.Incomes.tblIncomes)
        {
            _commandManager = commandManager;
            _controller = controller;
            _controller.UpdateAllLocalData += OnUpdateAllLocalData;
            _controller.UpdateSingleLocalData += OnUpdateSingleLocalData;
            _controller.RetrieveAllLocalData += OnRetrieveLocalData;

            _accountController = NinjectContainer.Kernel.Get<BaseController<Account>>();
        }

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


        private void OnUpdateSingleLocalData(Income updatedData)
        {
            Unprotect();

            var range = RowIndex[updatedData.TransactionCode];

            WriteWorksheetRow(range, updatedData);

            Sheet.Range[string.Format("tblIncomes[{0}]", Lang.ExpectedValue)].NumberFormat = ExcelNumberFormats.Accounting;
            Sheet.Range[string.Format("tblIncomes[{0}]", Lang.ActualValue)].NumberFormat = ExcelNumberFormats.Accounting;
 
            Protect();

        }

        private IEnumerable<Income> OnRetrieveLocalData()
        {
            return ReadFromWorksheet();
        }

        private void OnUpdateAllLocalData(IEnumerable<Income> updatedData)
        {
            Unprotect();

            Globals.ThisWorkbook.ThisApplication.ErrorCheckingOptions.NumberAsText = false;
            var data = updatedData.ToList();

            Table.SetDataBinding(data, "", DatabindCols);
            Table.Disconnect();
            

            //todo: manter a formatação dos demais campos para evitar que o usuário estrague a formatação do campo
            Sheet.Range[string.Format("tblIncomes[{0}]", Lang.ExpectedValue)].NumberFormat = ExcelNumberFormats.Accounting;
            Sheet.Range[string.Format("tblIncomes[{0}]", Lang.ActualValue)].NumberFormat = ExcelNumberFormats.Accounting;
            
            Table.Range.Columns.AutoFit();

            Globals.ThisWorkbook.ThisApplication.ErrorCheckingOptions.NumberAsText = true;

            Protect();
        }

        private IEnumerable<Income> ReadFromWorksheet()
        {
            var saidas = new List<Income>();

            try
            {
                object[,] dados = Table.Range.Value2;

                for (var row = 2; row <= dados.GetLength(0); row++)
                {
                    var entity = new Income();
                    ReadListObjectRow(row, dados, entity);

                    RowIndex.Set(entity.TransactionCode, (Range)Table.Range[row, Cols[Lang.TransactionCode]]);

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

        private void ReadWorksheetRow(Range row, Income e)
        {
            var r = row.EntireRow;
            e.TransactionDate = RangeUtils.ToDateTime(r.Cells[1, AbsCols[Lang.TransactionDate]]);
            e.Date = RangeUtils.ToDateTime(r.Cells[1, AbsCols[Lang.Date]]);
            e.ExpectedValue = RangeUtils.ToDecimal(r.Cells[1, AbsCols[Lang.ExpectedValue]]);
            e.AccountName = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.AccountName]]);
            e.Reason = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Reason]]);
            e.Place = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Place]]);
            e.ResponsibleName = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.ResponsibleName]]);
            e.CategoryName = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.CategoryName]]);
            e.Tags = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Tags]]);
            e.Quantity = RangeUtils.ToDecimal(r.Cells[1, AbsCols[Lang.Quantity]]);
            e.ActualValue = RangeUtils.ToDecimal(r.Cells[1, AbsCols[Lang.ActualValue]]);
            e.TransactionStatus = EnumTools.GetValueFromDescription<TransactionStatus>(RangeUtils.ToString(r.Cells[1, AbsCols[Lang.TransactionStatusDescription]]));
            e.EditStatus = EnumTools.GetValueFromDescription<EditStatus>(RangeUtils.ToString(r.Cells[1, AbsCols[Lang.EditStatus]]));
            e.DueDate = RangeUtils.ToDateTime(r.Cells[1, AbsCols[Lang.DueDate]]);
            e.IsRecurring = RangeUtils.ToBoolean(r.Cells[1, AbsCols[Lang.IsRecurring]]);
            e.MonthlyInterval = RangeUtils.ToInt(r.Cells[1, AbsCols[Lang.MonthlyInterval]]);
            e.RemainingInstallments = RangeUtils.ToInt(r.Cells[1, AbsCols[Lang.RemainingInstallments]]);
            e.AccountTransferCode = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.AccountTransferCode]]);
            e.CheckNumber = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.CheckNumber]]);
            e.SupportsDrillDown = RangeUtils.ToBoolean(r.Cells[1, AbsCols[Lang.SupportsDrillDown]]);
            e.TransactionGroup = RangeUtils.ToGuid(r.Cells[1, AbsCols[Lang.TransactionGroup]]);
            e.TransactionCode = RangeUtils.ToGuid(r.Cells[1, AbsCols[Lang.TransactionCode]]) ?? Guid.NewGuid();
            e.Remarks = RangeUtils.ToString(r.Cells[1, AbsCols[Lang.Remarks]]);
        }

        private void ReadListObjectRow(int row, object[,] dados, Income e)
        {
            e.TransactionDate = Parse.ToDateTime(dados[row, Cols[Lang.TransactionDate]]) ?? DateTime.Now;
            e.Date = Parse.ToDateTime(dados[row, Cols[Lang.Date]]);
            e.ExpectedValue = Parse.ToDouble(dados[row, Cols[Lang.ExpectedValue]]);
            e.AccountName = Parse.ToString(dados[row, Cols[Lang.AccountName]]);
            e.Reason = Parse.ToString(dados[row, Cols[Lang.Reason]]);
            e.Place = Parse.ToString(dados[row, Cols[Lang.Place]]);
            e.ResponsibleName = Parse.ToString(dados[row, Cols[Lang.ResponsibleName]]);
            e.CategoryName = Parse.ToString(dados[row, Cols[Lang.CategoryName]]);
            e.Tags = Parse.ToString(dados[row, Cols[Lang.Tags]]);
            e.Quantity = Parse.ToDecimal(dados[row, Cols[Lang.Quantity]]);
            e.ActualValue = Parse.ToDouble(dados[row, Cols[Lang.ActualValue]]);
            e.TransactionStatus = EnumTools.GetValueFromDescription<TransactionStatus>(Parse.ToString(dados[row, Cols[Lang.TransactionStatusDescription]]));
            e.EditStatus = EnumTools.GetValueFromDescription<EditStatus>(Parse.ToString(dados[row, Cols[Lang.EditStatus]]));
            e.DueDate = Parse.ToDateTime(dados[row, Cols[Lang.DueDate]]);
            e.IsRecurring = Parse.ToBoolean(dados[row, Cols[Lang.IsRecurring]]);
            e.MonthlyInterval = Parse.ToInt(dados[row, Cols[Lang.MonthlyInterval]]);
            e.RemainingInstallments = Parse.ToInt(dados[row, Cols[Lang.RemainingInstallments]]);
            e.AccountTransferCode = Parse.ToString(dados[row, Cols[Lang.AccountTransferCode]]);
            e.CheckNumber = Parse.ToString(dados[row, Cols[Lang.CheckNumber]]);
            e.SupportsDrillDown = Parse.ToBoolean(dados[row, Cols[Lang.SupportsDrillDown]]);
            e.TransactionGroup = Parse.ToGuid(dados[row, Cols[Lang.TransactionGroup]]);
            e.TransactionCode = Parse.ToGuid(dados[row, Cols[Lang.TransactionCode]]) ?? Guid.NewGuid();
            e.Remarks = Parse.ToString(dados[row, Cols[Lang.Remarks]]);
        }

        private void WriteWorksheetRow(Range row, Income e)
        {
            var r = row.EntireRow;
            r.Cells[1, AbsCols[Lang.TransactionDate]].Value2 = e.TransactionDate ?? r.Cells[1, AbsCols[Lang.TransactionDate]].Value2;
            r.Cells[1, AbsCols[Lang.Date]].Value2 = e.Date ?? r.Cells[1, AbsCols[Lang.Date]].Value2;
            r.Cells[1, AbsCols[Lang.ExpectedValue]].Value2 = e.ExpectedValue ?? r.Cells[1, AbsCols[Lang.ExpectedValue]].Value2;
            r.Cells[1, AbsCols[Lang.AccountName]].Value2 = e.AccountName ?? r.Cells[1, AbsCols[Lang.AccountName]].Value2;
            r.Cells[1, AbsCols[Lang.Reason]].Value2 = e.Reason ?? r.Cells[1, AbsCols[Lang.Reason]].Value2;
            r.Cells[1, AbsCols[Lang.Place]].Value2 = e.Place ?? r.Cells[1, AbsCols[Lang.Place]].Value2;
            r.Cells[1, AbsCols[Lang.ResponsibleName]].Value2 = e.ResponsibleName ?? r.Cells[1, AbsCols[Lang.ResponsibleName]].Value2;
            r.Cells[1, AbsCols[Lang.CategoryName]].Value2 = e.CategoryName ?? r.Cells[1, AbsCols[Lang.CategoryName]].Value2;
            r.Cells[1, AbsCols[Lang.Tags]].Value2 = e.Tags ?? r.Cells[1, AbsCols[Lang.Tags]].Value2;
            r.Cells[1, AbsCols[Lang.Quantity]].Value2 = e.Quantity ?? r.Cells[1, AbsCols[Lang.Quantity]].Value2;
            r.Cells[1, AbsCols[Lang.ActualValue]].Value2 = e.ActualValue ?? r.Cells[1, AbsCols[Lang.ActualValue]].Value2;
            r.Cells[1, AbsCols[Lang.TransactionStatusDescription]].Value2 = e.TransactionStatusDescription ?? r.Cells[1, AbsCols[Lang.TransactionStatusDescription]].Value2;
            r.Cells[1, AbsCols[Lang.EditStatus]].Value2 = e.EditStatus.ToString();
            r.Cells[1, AbsCols[Lang.DueDate]].Value2 = e.DueDate ?? r.Cells[1, AbsCols[Lang.DueDate]].Value2;
            r.Cells[1, AbsCols[Lang.IsRecurring]].Value2 = e.IsRecurring ?? r.Cells[1, AbsCols[Lang.IsRecurring]].Value2;
            r.Cells[1, AbsCols[Lang.MonthlyInterval]].Value2 = e.MonthlyInterval ?? r.Cells[1, AbsCols[Lang.MonthlyInterval]].Value2;
            r.Cells[1, AbsCols[Lang.RemainingInstallments]].Value2 = e.RemainingInstallments ?? r.Cells[1, AbsCols[Lang.RemainingInstallments]].Value2;
            r.Cells[1, AbsCols[Lang.AccountTransferCode]].Value2 = e.AccountTransferCode ?? r.Cells[1, AbsCols[Lang.AccountTransferCode]].Value2;
            r.Cells[1, AbsCols[Lang.CheckNumber]].Value2 = e.CheckNumber ?? r.Cells[1, AbsCols[Lang.CheckNumber]].Value2;
            r.Cells[1, AbsCols[Lang.SupportsDrillDown]].Value2 = e.SupportsDrillDown ?? r.Cells[1, AbsCols[Lang.SupportsDrillDown]].Value2;
            r.Cells[1, AbsCols[Lang.TransactionGroup]].Value2 = e.TransactionGroup ?? r.Cells[1, AbsCols[Lang.TransactionGroup]].Value2;
            r.Cells[1, AbsCols[Lang.TransactionCode]].Value2 = e.TransactionCode.ToString();
            r.Cells[1, AbsCols[Lang.Remarks]].Value2 = e.Remarks ?? r.Cells[1, AbsCols[Lang.Remarks]].Value2;
        }

        public class Events
        {
            private Excel.Range _activeRange;
            private readonly IncomeWorksheet _parent;

            [Inject]
            public Events(IncomeWorksheet parent)
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

                    Guid codLancamento = RangeUtils.ToGuid(targetRange.EntireRow.Cells[1, _parent.AbsCols[Lang.TransactionCode]]);
                    var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                   _parent.ReadWorksheetRow(targetRange, entity);

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

                    var codLancamento = RangeUtils.ToGuid(_activeRange.EntireRow.Cells[1, _parent.AbsCols[Lang.TransactionCode]]);
                    if (codLancamento == null)
                        return;

                    var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                    _commandManager.UpdateSidePanel(entity);
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
            private IncomeWorksheet _parent;

            [Inject]
            public ContextMenus(IncomeWorksheet parent)
            {
                _parent = parent;
                this.Prepare();
            }

            /// <summary>
            /// Montar quatro opções no menu de contexto, com seus devidos ícones e event handlers associados.
            /// </summary>
            private void Prepare()
            {
                //fonte dos ícones (faceid): http://www.kebabshopblues.co.uk/2007/01/04/visual-studio-2005-tools-for-office-commandbarbutton-faceid-property/

                //criar um novo command bar do tipo popup para acomodar os itens criados abaixo.
                _commandBar = Globals.ThisWorkbook.Application.CommandBars.Add("IncomeContextMenu", Office.MsoBarPosition.msoBarPopup, false, true);

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

                //_menuSalvar = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                //_menuSalvar.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                //_menuSalvar.Caption = "Salvar...";
                //_menuSalvar.FaceId = 1975;
                //_menuSalvar.Tag = "2";

                _menuRemover = (Office.CommandBarButton)_commandBar.Controls.Add(1);
                _menuRemover.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                _menuRemover.Caption = "Remove Transaction...";
                _menuRemover.FaceId = 0478;
                _menuRemover.Tag = "3";


                //_menuEdit.Click += MenuEditClick;
                //_menuSalvar.Click += MenuSaveClick;
                //_menuRemover.Click += MenuRemoveClick;
                _menuInserir.Click += this.MenuCreateClick;
            }

            private void MenuCreateClick(CommandBarButton ctrl, ref bool canceldefault)
            {
                _parent.Unprotect();

                //create a new Income
                var newIncome = new Income();
                newIncome.Date = DateTime.Now;
                newIncome.TransactionDate = DateTime.Now;
                newIncome.TransactionCode = Guid.NewGuid();
                newIncome.EditStatus = EditStatus.Created;
                newIncome.TransactionStatus = TransactionStatus.Unknown;

                //solicitar ao controller que aceite os novos dados.
                _controller.AcceptData(newIncome);

                //configurar a linha nova da planilha com valores default.
                var newRow = _parent.Table.ListRows.Add();
                newRow.Range[1, _parent.Cols[Lang.Date]].Value2 = newIncome.Date;
                newRow.Range[1, _parent.Cols[Lang.TransactionDate]].Value2 = newIncome.TransactionDate;
                newRow.Range[1, _parent.Cols[Lang.TransactionCode]].Value2 = newIncome.TransactionCode.ToString();
                newRow.Range[1, _parent.Cols[Lang.EditStatus]].Value2 = newIncome.TransactionStatusDescription;
                newRow.Range[1, _parent.Cols[Lang.TransactionStatusDescription]].Value2 = newIncome.TransactionStatus.GetDescription();

                //atualizar o índice de linhas com esta nova saída.
                _parent.RowIndex.Set(newIncome.TransactionCode, (Range)newRow.Range[1, _parent.Cols[Lang.TransactionCode]]);

                newRow.Range[1, 1].Select();

                _parent.Protect();
            }


            private void MenuEditClick(CommandBarButton ctrl, ref bool canceldefault)
            {
                Guid codLancamento = RangeUtils.ToGuid(_activeRange.EntireRow.Cells[1, _parent.AbsCols[Lang.TransactionCode]]);

                
                var entity = _controller.CurrentSessionData.FirstOrDefault(x => x.TransactionCode == codLancamento);
                _commandManager.UpdateSidePanel(entity);

            }


            public void ShowContextMenu(Excel.Range target, ref bool cancel)
            {
                _activeRange = target;
                //_commandBar.ShowPopup();
                Globals.ThisWorkbook.Application.CommandBars["IncomeContextMenu"].ShowPopup();
                cancel = true;
            }
        }

    }


}