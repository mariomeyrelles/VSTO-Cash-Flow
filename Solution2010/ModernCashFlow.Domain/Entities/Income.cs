using System;
using System.ComponentModel;
using ModernCashFlow.Domain.BaseInterfaces;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Entities
{
    public class Income : DomainBase, IMoneyTransaction
    {
        public Income()
        {
            this.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }

        public Income(Guid identity)
        {
            this.TransactionCode = identity;
            this.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
        }


        #region Propriedades Básicas

        ///  /// <summary>
        /// Campo obrigatório para identificar unicamente um registro na planilha.
        /// </summary>
        [LocalizableColumnName]
        public Guid TransactionCode { get; set; }


        /// <summary>
        /// Campo opcional útil para marcar quais transações fazem parte de um lançamento, principalmente no caso de 
        /// de transações parceladas.
        /// </summary>
        [LocalizableColumnName]
        public Guid? TransactionGroup { get; set; }

        /// <summary>
        /// Campo obrigatório. A data de lançamento é carimbada com Datetime.Now na primeira vez que eu criar um lançamento com sucesso, 
        /// isto é, sem erros que impeçam o sistema de considerar esta saída no fluxo de caixa. 
        /// </summary>
        [LocalizableColumnName]
        public DateTime? TransactionDate { get; set; }


        public double? TransactionDate_OA
        {
            get
            {
                var dateTime = this.TransactionDate;
                if (dateTime != null) return dateTime.Value.ToOADate();
                return null;
            }
        }

       

        private DateTime? _date;

        /// <summary>
        /// Campo obrigatório. A data indica o dia exato em que o dinheiro saiu ou sairá da conta. Vale notar que em casos de cartão de crédito, 
        /// a data efetiva é a data de vencimento da fatura. É necessário observar o status do lançamento para saber se o dinheiro efetivamente saiu.
        /// Observar o campo Data de Vencimento para saber se o lançamento está em dia ou em atraso.
        /// 
        /// Esta propriedade dispara INPC.
        /// </summary>
        [LocalizableColumnName]
        public DateTime? Date
        {
            get { return _date.Today(); }
            set { SetField(ref _date, value, () => Date); }
        }

        public double? Date_OA
        {
            get
            {
                var dateTime = this.Date;
                if (dateTime != null) return dateTime.Value.ToOADate();
                return null;
            }
        }

        

        private decimal? _expectedValue;

        /// <summary>
        /// Campo obrigatório que indica o valor que se espera que seja esta saída, sem juros e sem descontos. 
        /// 
        /// Esta propriedade dispara INPC.
        /// </summary>
        [LocalizableColumnName]
        public decimal? ExpectedValue
        {
            get { return _expectedValue; }
            set { SetField(ref _expectedValue, value, () => ExpectedValue); }
        }


        /// <summary>
        /// Campo opcional que indica o valor pago após juros e/ou descontos.
        /// </summary>
        [LocalizableColumnName]
        public decimal? ActualValue { get; set; }



        public decimal Value
        {
            get
            {
                if (this.ActualValue.HasValue)
                {
                    return this.ActualValue.Value;
                }
                return this.ExpectedValue ?? 0.0m;
            }
        }

        /// <summary>
        /// Campo obrigatório que indica o ID da conta que deverá a ser debitada este valor. Pode ser opcional caso só se tenha uma 
        /// única conta.
        /// </summary>
        public int AccountId { get; set; }



        /// <summary>
        /// O nome amigável da conta que deverá ser debitada.
        /// </summary>
        [LocalizableColumnName]
        public string AccountName
        {
            get { return _nomeConta; }
            set { SetField(ref _nomeConta, value, () => AccountName); }
        }

        private string _nomeConta;

        /// <summary>
        /// Justificativa desta saída de fluxo de caixa.
        /// </summary>
        [LocalizableColumnName]
        public string Reason { get; set; }

        /// <summary>
        /// Local físico onde este lançamento foi realizado.
        /// </summary>
        [LocalizableColumnName]
        public string Place { get; set; }

        /// <summary>
        /// Pessoa que efetuou este lançamento. 
        /// </summary>
        [LocalizableColumnName]
        public string ResponsibleName { get; set; }

        /// <summary>
        /// Campo opcional que serve para classificar este lançamento entre os diversos tipos de categoria, como moradia, transporte, etc.
        /// </summary>
        [LocalizableColumnName]
        public string CategoryName { get; set; }

        /// <summary>
        /// Palavras-chave opcionais que ajudam a classificar os lançamentos de uma forma customizada pelo usuário final.
        /// </summary>
        [LocalizableColumnName]
        public string Tags { get; set; }

        /// <summary>
        /// Campo opcional que indica uma eventual quantidade de itens adquiridos nesta saída. Por exemplo, o valor informado pode 
        /// se referir a 10 sorvetes, duas passagens, etc...
        /// </summary>
        [LocalizableColumnName]
        public decimal? Quantity { get; set; }

      

        /// <summary>
        /// Indica, de modo geral, se a saída pode ser usada ou não no cálculo de fluxo de caixa.
        /// </summary>

        public TransactionStatus TransactionStatus { get; set; }

        /// <summary>
        /// Descrição amigável do Status de Lançamento usando o atributo [Description] de cada item do Enum.
        /// </summary>
        [LocalizableColumnName]
        public string TransactionStatusDescription
        {
            get { return this.TransactionStatus.GetDescription(); }
        }

        /// <summary>
        /// Ajuda a controlar quais itens estão em edição na planilha.
        /// </summary>
        [LocalizableColumnName]
        public EditStatus EditStatus { get; set; }


        /// <summary>
        /// No caso de despesas mensais recorrentes, indica o dia do mês que a conta vence.
        /// </summary>
        [LocalizableColumnName]
        public DateTime? DueDate { get; set; }

        public double? DueDate_OA
        {
            get
            {
                var dateTime = this.DueDate;
                if (dateTime != null) return dateTime.Value.ToOADate();
                return null;
            }
        }

        /// <summary>
        /// Indica que este lançamento representa o pagamento de despesa mensal recorrente.
        /// </summary>
        [LocalizableColumnName]
        public bool? IsRecurring { get; set; }

        /// <summary>
        /// Indica o intervalo de meses que a despesa é lançada. Serve para atender cenários como pagamentos de contas que vencem a cada 6 meses,
        /// 12 meses ou 3 meses (como alguns impostos).
        /// </summary>
        [LocalizableColumnName]
        public int? MonthlyInterval { get; set; }

        /// <summary>
        /// Número de parcelas restantes de um determinado parcelamento. Este número reflete somente parcelas que ainda vão vencer, já
        /// que esta saída indica um pagamento de parcela, descontando assim do total de parcelas faltantes. Por exemplo, se eu lançar 
        /// esta saída como pagamento de uma parcela de uma compra feita em 10x e este for o primeiro pagamento, este campo deverá mostrar
        /// o valor 9, isto é, foi paga a primeira parcela e ainda faltam 9.
        /// </summary>
        [LocalizableColumnName]
        public int? RemainingInstallments { get; set; }

        /// <summary>
        /// Indica que este saída corresponde na verdade a uma transferência entre contas. Deve haver uma entrada correspondente com o 
        /// mesmo valor e características para que não haja erros.
        /// </summary>
        [LocalizableColumnName]
        public string AccountTransferCode { get; set; }

        /// <summary>
        /// Número do cheque utilizado para pagar custear esta saída. Vale notar que os campos de data e status devem ser manipulados
        /// corretamente para tratar os casos de cheques pré-datados também.
        /// </summary>
        [LocalizableColumnName]
        public string CheckNumber { get; set; }

        /// <summary>
        /// Quando verdadeiro, indica que este lançamento pode ser composto de sub-lançamentos. O caso clássico é a compra de supermercado,
        /// onde os itens comprados podem ser classificados em várias categorias.
        /// </summary>
        [LocalizableColumnName]
        public bool? SupportsDrillDown { get; set; }

        /// <summary>
        /// Campo textual livre para guardar qualquer observação pertinente a esta saída.
        /// </summary>
        [LocalizableColumnName]
        public string Remarks { get; set; }

        /// <summary>
        /// Indica se este lançamento pode ser considerado para o fluxo de caixa.
        /// </summary>
        public bool IsValid
        {
            get
            {
                //todo: ver como devo tratar contas
                if (!Date.HasValue || !ExpectedValue.HasValue || AccountName == null)
                {
                    ValidationMessage = "Este lançamento está incompleto e não pode ser considerado para o fluxo de caixa. Os campos data, valor e conta são obrigatórios.";
                    return false;
                }

                ValidationMessage = string.Empty;
                return true;
            }
        }

        public bool IsTransient
        {
            get { return EditStatus == EditStatus.Created; }
        }

        /// <summary>
        /// Quando há algum erro evidente, este campo guarda a mensagem de validação.
        /// </summary>
        public string ValidationMessage { get; set; }



        #endregion


        #region Validação de mudanças de propriedades

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            VerifyPropertyChanges();
        }

        public override void NotifyPropertyChange()
        {
            VerifyPropertyChanges();
        }

        private void VerifyPropertyChanges()
        {
            if (Date.HasValue == false)
                return;

            EditStatus = this.IsValid ? EditStatus.Complete : EditStatus.Incomplete;

            switch (TransactionStatus)
            {
                case TransactionStatus.Unknown:
                case TransactionStatus.Scheduled:
                case TransactionStatus.Pending:
                    break;
                case TransactionStatus.OK:
                case TransactionStatus.Suspended:
                case TransactionStatus.Canceled:
                case TransactionStatus.Invalid:
                    return;
            }

            var transactionDate = this.Date.Value.Today();
            var today = DateTime.Now.Today();

            if (transactionDate <= today)
            {
                this.TransactionStatus = TransactionStatus.Pending;
            }

            if (transactionDate > today)
            {
                this.TransactionStatus = TransactionStatus.Scheduled;
            }
        }


        public string PanelMessage
        {
            get { return MountPanelMessage(); }
        }

        private string MountPanelMessage()
        {

            if (!IsValid)
            {
                return ValidationMessage;
            }

            var now = DateTime.Now.Today();
            var transactionDate = Date.Value.Today();

            switch (this.TransactionStatus)
            {
                case TransactionStatus.Scheduled:
                    if (now < transactionDate)
                        return string.Format("Este lançamento vence daqui a {0} dias. ", (transactionDate - now).Days);
                    if (now > transactionDate)
                        return string.Format("Este lançamento está em atraso há {0} dias.", (now - transactionDate).Days);

                    break;
                case TransactionStatus.Pending:
                    if (now == transactionDate)
                        return "Este lançamento vence hoje.";
                    if (now > transactionDate)
                        return string.Format("Este lançamento está em atraso há {0} dias. ", (now - transactionDate).Days);
                    break;
                case TransactionStatus.OK:
                    return string.Format("Este lançamento está OK.");
                case TransactionStatus.Suspended:
                    return "Este lançamento está suspenso e portanto não está sendo considerado no fluxo de caixa.";
                case TransactionStatus.Canceled:
                    return "Este lançamento foi cancelado e portanto não está sendo considerado no fluxo de caixa.";
                case TransactionStatus.Invalid:
                case TransactionStatus.Unknown:
                    return "Este lançamento não está sendo considerado para o fluxo de caixa pois não ainda possui status definido.";
                default:
                    return "";
            }

            return "-";
        }

        #endregion

    }
}