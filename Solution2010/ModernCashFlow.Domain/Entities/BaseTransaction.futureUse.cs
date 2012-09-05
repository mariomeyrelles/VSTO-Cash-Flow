using System;
using ModernCashFlow.Tools;

namespace ModernCashFlow.Domain.Entities
{
    public partial class BaseTransaction
    {
        ///// <summary>
        ///// No caso de despesas mensais recorrentes, indica o dia do mês que a conta vence.
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public DateTime? DueDate { get; set; }

        ///// <summary>
        ///// Indica que este lançamento representa o pagamento de despesa mensal recorrente.
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public bool? IsRecurring { get; set; }

        ///// <summary>
        ///// Indica o intervalo de meses que a despesa é lançada. Serve para atender cenários como pagamentos de contas que vencem a cada 6 meses,
        ///// 12 meses ou 3 meses (como alguns impostos).
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public int? MonthlyInterval { get; set; }

        ///// <summary>
        ///// Número de parcelas restantes de um determinado parcelamento. Este número reflete somente parcelas que ainda vão vencer, já
        ///// que esta saída indica um pagamento de parcela, descontando assim do total de parcelas faltantes. Por exemplo, se eu lançar 
        ///// esta saída como pagamento de uma parcela de uma compra feita em 10x e este for o primeiro pagamento, este campo deverá mostrar
        ///// o valor 9, isto é, foi paga a primeira parcela e ainda faltam 9.
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public int? RemainingInstallments { get; set; }

        ///// <summary>
        ///// Quando verdadeiro, indica que este lançamento pode ser composto de sub-lançamentos. O caso clássico é a compra de supermercado,
        ///// onde os itens comprados podem ser classificados em várias categorias.
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public bool? SupportsDrillDown { get; set; }

        ///// <summary>
        ///// Campo opcional útil para marcar quais transações fazem parte de um lançamento, principalmente no caso de 
        ///// de transações parceladas.
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public Guid? TransactionGroup { get; set; }

        ///// <summary>
        ///// Indica que este saída corresponde na verdade a uma transferência entre contas. Deve haver uma entrada correspondente com o 
        ///// mesmo valor e características para que não haja erros.
        ///// </summary>
        //[LocalizableColumnName]
        //[Ignore("Future Use")]
        //public string AccountTransferCode
        //{
        //    get { return _accountTransferCode; }
        //    set { _accountTransferCode = value; }
        //}
        //public double? DueDate_OA
        //{
        //    get
        //    {
        //        var dateTime = this.DueDate;
        //        if (dateTime != null) return dateTime.Value.ToOADate();
        //        return null;
        //    }
        //}
      
    }
}