using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Tools.Excel;
using ModernCashFlow.Domain;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Tools;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Diagnostics;
using Ninject;

namespace ModernCashFlow.Excel2010.WorksheetLogic
{
    public class TestesLeitura
    {
        public TestesLeitura()
        {
            var controller = NinjectContainer.Kernel.Get<BaseController<Saida>>();
            controller.UpdateLocalData += OnDataChanged;
            controller.RetrieveLocalData += OnDataNeeded;
        }

        IEnumerable<Saida> OnDataNeeded()
        {
            return this.ReadFromWorksheet3();
        }

        void OnDataChanged(IEnumerable<Saida> obj)
        {

        }

        public void Teste()
        {
            MessageBox.Show("Teste - Saída Wks");

        }

        public IEnumerable<Saida> ReadFromWorksheet()
        {
            var itensLidos = new List<Saida>();
            var tbl = Globals.Sheet6.tblSaidas;

            try
            {
                //criar uma instância da classe entidade Produto para cada linha encontrada na tabela.
                foreach (Excel.ListRow currentRow in tbl.ListRows)
                {
                    var dataLancamento = RangeUtils.ReadColumn<DateTime>("Data de Lançamento", tbl.HeaderRowRange, currentRow);
                    var dataPrevista = RangeUtils.ReadColumn<DateTime>("Data Prevista", tbl.HeaderRowRange, currentRow);
                    var dataEfetiva = RangeUtils.ReadColumn<DateTime>("Data Efetiva", tbl.HeaderRowRange, currentRow);
                    var conta = RangeUtils.ReadColumn<string>("Conta", tbl.HeaderRowRange, currentRow);
                    var motivo = RangeUtils.ReadColumn<string>("Motivo", tbl.HeaderRowRange, currentRow);
                    var local = RangeUtils.ReadColumn<string>("Local", tbl.HeaderRowRange, currentRow);
                    var responsavel = RangeUtils.ReadColumn<string>("Responsável", tbl.HeaderRowRange, currentRow);
                    var categoria = RangeUtils.ReadColumn<string>("Categoria", tbl.HeaderRowRange, currentRow);
                    var qtd = RangeUtils.ReadColumn<decimal>("Quantidade", tbl.HeaderRowRange, currentRow);
                    var valorEfetivo = RangeUtils.ReadColumn<decimal>("Valor Efetivo", tbl.HeaderRowRange, currentRow);
                    var statusConfirmacao = RangeUtils.ReadColumn<string>("Status de Confirmação", tbl.HeaderRowRange, currentRow);
                    var diaVcto = RangeUtils.ReadColumn<int>("Dia de Vencimento", tbl.HeaderRowRange, currentRow);
                    var despesa = RangeUtils.ReadColumn<bool>("Despesa", tbl.HeaderRowRange, currentRow);
                    var periodicidadeMensal = RangeUtils.ReadColumn<int>("Periodicidade Mensal", tbl.HeaderRowRange, currentRow);
                    var parcelasRestantes = RangeUtils.ReadColumn<int>("Parcelas Restantes", tbl.HeaderRowRange, currentRow);
                    var codTransf = RangeUtils.ReadColumn<Guid>("Código da Transferência", tbl.HeaderRowRange, currentRow);
                    var numeroCheque = RangeUtils.ReadColumn<string>("Número do Cheque", tbl.HeaderRowRange, currentRow);
                    var suportaDrillDown = RangeUtils.ReadColumn<bool>("Suporta DrillDown", tbl.HeaderRowRange, currentRow);
                    var codLancto = RangeUtils.ReadColumn<Guid>("Código do Lançamento", tbl.HeaderRowRange, currentRow);
                    var codTransacao = RangeUtils.ReadColumn<Guid>("Código da Transação", tbl.HeaderRowRange, currentRow);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return null;
        }

        public void ReadFromWorksheet2()
        {
            //ListObject named "tblSaidas"
            var tbl = Globals.Sheet6.tblSaidas;

            try
            {
                foreach (Excel.ListRow row in tbl.ListRows)
                {
                    var dataLancamento = RangeUtils.ToDateTime(row.Range[1,1]);
                    var dataPrevista = RangeUtils.ToDateTime(row.Range[1, 2]);
                    var dataEfetiva = RangeUtils.ToDateTime(row.Range[1, 3]);
                    var valorPrevisto = RangeUtils.ToDateTime(row.Range[1, 3]);
                    var conta = RangeUtils.ToString(row.Range[1, 4]);
                    var motivo = RangeUtils.ToString(row.Range[1, 5]);
                    var local = RangeUtils.ToString(row.Range[1, 6]);
                    var responsavel = RangeUtils.ToString(row.Range[1, 7]);
                    var categoria = RangeUtils.ToString(row.Range[1, 8]);
                    var tags = RangeUtils.ToString(row.Range[1, 8]);
                    var qtd = RangeUtils.ToDecimal(row.Range[1, 9]);
                    var valorEfetivo = RangeUtils.ToDecimal(row.Range[1, 10]);
                    var statusConfirmacao = RangeUtils.ToString(row.Range[1, 11]);
                    var diaVcto = RangeUtils.ToInt(row.Range[1, 12]);
                    var despesa = RangeUtils.ToBoolean(row.Range[1, 13]);
                    var periodicidadeMensal = RangeUtils.ToInt(row.Range[1, 14]);
                    var parcelasRestantes = RangeUtils.ToInt(row.Range[1, 15]);
                    var codTransf = RangeUtils.ToGuid(row.Range[1, 16]);
                    var numeroCheque = RangeUtils.ToString(row.Range[1, 17]);
                    var suportaDrillDown = RangeUtils.ToBoolean(row.Range[1, 18]);
                    var codTransacao = RangeUtils.ToGuid(row.Range[1, 20]);
                    var codLancto = RangeUtils.ToGuid(row.Range[1, 19]);
                    var obs = RangeUtils.ToString(row.Range[1, 21]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        public List<Saida> ReadFromWorksheet3()
        {
            var saidas = new List<Saida>();

            //ListObject named "tblSaidas"
            var tbl = Globals.Sheet6.tblSaidas;

            try
            {
                object[,] dados = tbl.Range.Value;

                for (int row = 2; row < dados.GetLength(0); row++)
                {

                    var dataLancamento = Parse.ToDateTime(dados[row, 1]) ?? DateTime.Now;
                    var dataPrevista = Parse.ToDateTime(dados[row, 2]);
                    var dataEfetiva = Parse.ToDateTime(dados[row, 3]);
                    var valorPrevisto = Parse.ToDouble(dados[row, 4]);
                    var conta = Parse.ToString(dados[row, 5]);
                    var motivo = Parse.ToString(dados[row, 6]);
                    var local = Parse.ToString(dados[row, 7]);
                    var responsavel = Parse.ToString(dados[row, 8]);
                    var categoria = Parse.ToString(dados[row, 9]);
                    var tags = Parse.ToString(dados[row, 10]);
                    var qtd = Parse.ToDecimal(dados[row, 11]);
                    var valorEfetivo = Parse.ToDouble(dados[row, 12]);
                    var statusConfirmacao = Parse.ToString(dados[row, 13]);
                    var diaVcto = Parse.ToInt(dados[row, 14]);
                    var despesa = Parse.ToBoolean(dados[row, 15]);
                    var periodicidadeMensal = Parse.ToInt(dados[row, 16]);
                    var parcelasRestantes = Parse.ToInt(dados[row, 17]);
                    var codTransf = Parse.ToString(dados[row, 18]);
                    var numeroCheque = Parse.ToString(dados[row, 19]);
                    var suportaDrillDown = Parse.ToBoolean(dados[row, 20]);
                    var codTransacao = Parse.ToGuid(dados[row, 21]);
                    var codLancto = Parse.ToGuid(dados[row, 22]);
                    var obs = Parse.ToString(dados[row, 23]);

                    var saida = new Saida();
                    saida.DataLancamento = dataLancamento;
                    saida.DataPrevista = dataPrevista;
                    saida.DataEfetiva = dataEfetiva;
                    saida.ValorPrevisto = valorPrevisto;
                    saida.NomeConta = conta;
                    saida.Motivo = motivo;
                    saida.Local = local;
                    saida.Responsavel = responsavel;
                    saida.Categoria = categoria;
                    saida.Tags = tags;
                    saida.Quantidade = qtd;
                    saida.ValorEfetivo = valorEfetivo;
                    saida.StatusConfirmacao = statusConfirmacao;
                    saida.DiaVencimento = diaVcto;
                    saida.Despesa = despesa;
                    saida.PeriodicidadeMensal = periodicidadeMensal;
                    saida.ParcelasRestantes = parcelasRestantes;
                    saida.CodigoTransferencia = codTransf;
                    saida.NumeroCheque = numeroCheque;
                    saida.SuportaDrillDown = suportaDrillDown;
                    saida.CodigoTransacao = codTransacao;
                    saida.CodigoLancamento = codLancto;
                    saida.Observacoes = obs;
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
        
    }

    
}