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


        #region Propriedades B�sicas

        ///  /// <summary>
        /// Campo obrigat�rio para identificar unicamente um registro na planilha.
        /// </summary>
        [LocalizableColumnName]
        public Guid TransactionCode { get; set; }


        /// <summary>
        /// Campo opcional �til para marcar quais transa��es fazem parte de um lan�amento, principalmente no caso de 
        /// de transa��es parceladas.
        /// </summary>
        [LocalizableColumnName]
        public Guid? TransactionGroup { get; set; }

        /// <summary>
        /// Campo obrigat�rio. A data de lan�amento � carimbada com Datetime.Now na primeira vez que eu criar um lan�amento com sucesso, 
        /// isto �, sem erros que impe�am o sistema de considerar esta sa�da no fluxo de caixa. 
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
        /// Campo obrigat�rio. A data indica o dia exato em que o dinheiro saiu ou sair� da conta. Vale notar que em casos de cart�o de cr�dito, 
        /// a data efetiva � a data de vencimento da fatura. � necess�rio observar o status do lan�amento para saber se o dinheiro efetivamente saiu.
        /// Observar o campo Data de Vencimento para saber se o lan�amento est� em dia ou em atraso.
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
        /// Campo obrigat�rio que indica o valor que se espera que seja esta sa�da, sem juros e sem descontos. 
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
        /// Campo opcional que indica o valor pago ap�s juros e/ou descontos.
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
        /// Campo obrigat�rio que indica o ID da conta que dever� a ser debitada este valor. Pode ser opcional caso s� se tenha uma 
        /// �nica conta.
        /// </summary>
        public int AccountId { get; set; }



        /// <summary>
        /// O nome amig�vel da conta que dever� ser debitada.
        /// </summary>
        [LocalizableColumnName]
        public string AccountName
        {
            get { return _nomeConta; }
            set { SetField(ref _nomeConta, value, () => AccountName); }
        }

        private string _nomeConta;

        /// <summary>
        /// Justificativa desta sa�da de fluxo de caixa.
        /// </summary>
        [LocalizableColumnName]
        public string Reason { get; set; }

        /// <summary>
        /// Local f�sico onde este lan�amento foi realizado.
        /// </summary>
        [LocalizableColumnName]
        public string Place { get; set; }

        /// <summary>
        /// Pessoa que efetuou este lan�amento. 
        /// </summary>
        [LocalizableColumnName]
        public string ResponsibleName { get; set; }

        /// <summary>
        /// Campo opcional que serve para classificar este lan�amento entre os diversos tipos de categoria, como moradia, transporte, etc.
        /// </summary>
        [LocalizableColumnName]
        public string CategoryName { get; set; }

        /// <summary>
        /// Palavras-chave opcionais que ajudam a classificar os lan�amentos de uma forma customizada pelo usu�rio final.
        /// </summary>
        [LocalizableColumnName]
        public string Tags { get; set; }

        /// <summary>
        /// Campo opcional que indica uma eventual quantidade de itens adquiridos nesta sa�da. Por exemplo, o valor informado pode 
        /// se referir a 10 sorvetes, duas passagens, etc...
        /// </summary>
        [LocalizableColumnName]
        public decimal? Quantity { get; set; }

      

        /// <summary>
        /// Indica, de modo geral, se a sa�da pode ser usada ou n�o no c�lculo de fluxo de caixa.
        /// </summary>

        public TransactionStatus TransactionStatus { get; set; }

        /// <summary>
        /// Descri��o amig�vel do Status de Lan�amento usando o atributo [Description] de cada item do Enum.
        /// </summary>
        [LocalizableColumnName]
        public string TransactionStatusDescription
        {
            get { return this.TransactionStatus.GetDescription(); }
        }

        /// <summary>
        /// Ajuda a controlar quais itens est�o em edi��o na planilha.
        /// </summary>
        [LocalizableColumnName]
        public EditStatus EditStatus { get; set; }


        /// <summary>
        /// No caso de despesas mensais recorrentes, indica o dia do m�s que a conta vence.
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
        /// Indica que este lan�amento representa o pagamento de despesa mensal recorrente.
        /// </summary>
        [LocalizableColumnName]
        public bool? IsRecurring { get; set; }

        /// <summary>
        /// Indica o intervalo de meses que a despesa � lan�ada. Serve para atender cen�rios como pagamentos de contas que vencem a cada 6 meses,
        /// 12 meses ou 3 meses (como alguns impostos).
        /// </summary>
        [LocalizableColumnName]
        public int? MonthlyInterval { get; set; }

        /// <summary>
        /// N�mero de parcelas restantes de um determinado parcelamento. Este n�mero reflete somente parcelas que ainda v�o vencer, j�
        /// que esta sa�da indica um pagamento de parcela, descontando assim do total de parcelas faltantes. Por exemplo, se eu lan�ar 
        /// esta sa�da como pagamento de uma parcela de uma compra feita em 10x e este for o primeiro pagamento, este campo dever� mostrar
        /// o valor 9, isto �, foi paga a primeira parcela e ainda faltam 9.
        /// </summary>
        [LocalizableColumnName]
        public int? RemainingInstallments { get; set; }

        /// <summary>
        /// Indica que este sa�da corresponde na verdade a uma transfer�ncia entre contas. Deve haver uma entrada correspondente com o 
        /// mesmo valor e caracter�sticas para que n�o haja erros.
        /// </summary>
        [LocalizableColumnName]
        public string AccountTransferCode { get; set; }

        /// <summary>
        /// N�mero do cheque utilizado para pagar custear esta sa�da. Vale notar que os campos de data e status devem ser manipulados
        /// corretamente para tratar os casos de cheques pr�-datados tamb�m.
        /// </summary>
        [LocalizableColumnName]
        public string CheckNumber { get; set; }

        /// <summary>
        /// Quando verdadeiro, indica que este lan�amento pode ser composto de sub-lan�amentos. O caso cl�ssico � a compra de supermercado,
        /// onde os itens comprados podem ser classificados em v�rias categorias.
        /// </summary>
        [LocalizableColumnName]
        public bool? SupportsDrillDown { get; set; }

        /// <summary>
        /// Campo textual livre para guardar qualquer observa��o pertinente a esta sa�da.
        /// </summary>
        [LocalizableColumnName]
        public string Remarks { get; set; }

        /// <summary>
        /// Indica se este lan�amento pode ser considerado para o fluxo de caixa.
        /// </summary>
        public bool IsValid
        {
            get
            {
                //todo: ver como devo tratar contas
                if (!Date.HasValue || !ExpectedValue.HasValue || AccountName == null)
                {
                    ValidationMessage = "Este lan�amento est� incompleto e n�o pode ser considerado para o fluxo de caixa. Os campos data, valor e conta s�o obrigat�rios.";
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
        /// Quando h� algum erro evidente, este campo guarda a mensagem de valida��o.
        /// </summary>
        public string ValidationMessage { get; set; }



        #endregion


        #region Valida��o de mudan�as de propriedades

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
                        return string.Format("Este lan�amento vence daqui a {0} dias. ", (transactionDate - now).Days);
                    if (now > transactionDate)
                        return string.Format("Este lan�amento est� em atraso h� {0} dias.", (now - transactionDate).Days);

                    break;
                case TransactionStatus.Pending:
                    if (now == transactionDate)
                        return "Este lan�amento vence hoje.";
                    if (now > transactionDate)
                        return string.Format("Este lan�amento est� em atraso h� {0} dias. ", (now - transactionDate).Days);
                    break;
                case TransactionStatus.OK:
                    return string.Format("Este lan�amento est� OK.");
                case TransactionStatus.Suspended:
                    return "Este lan�amento est� suspenso e portanto n�o est� sendo considerado no fluxo de caixa.";
                case TransactionStatus.Canceled:
                    return "Este lan�amento foi cancelado e portanto n�o est� sendo considerado no fluxo de caixa.";
                case TransactionStatus.Invalid:
                case TransactionStatus.Unknown:
                    return "Este lan�amento n�o est� sendo considerado para o fluxo de caixa pois n�o ainda possui status definido.";
                default:
                    return "";
            }

            return "-";
        }

        #endregion

    }
}