using System;
using System.ComponentModel;

namespace ModernCashFlow.Domain.Entities
{
    /// <summary>
    /// Status geral de um determinado lançamento. De acordo com o status é possível decidir de já é possível considerar ou não
    /// este lançamento para montar o fluxo de caixa.
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Status que indica que não se sabe ainda o status do lançamento.
        /// </summary>
        [Description("-")]
        Unknown = 0,
        /// <summary>
        /// Quando o lançamento está previsto, espera-se que a data do lançamento aconteça na data prevista.
        /// </summary>
        [Description("Previsto")]
        Scheduled = 1,
        /// <summary>
        /// O lançamento é considerando pendente quando a data de vencimento já passou e não foi dado baixa no lançamento.
        /// </summary>
        [Description("Pendente")]
        Pending = 2,
        /// <summary>
        /// O lançamento está OK quando o usário confirma o lançamento adequadamente.
        /// </summary>
        [Description("OK")]
        OK = 3,
        /// <summary>
        /// O lançamento está suspenso quando o usuário decide não lançar mais este valor. Para efeitos de fluxo de caixa
        /// este lançamento não existirá mais.
        /// </summary>
        [Description("Suspenso")]
        Suspended = 4,
        /// <summary>
        /// O lançamento é cancelado quando se tem a certeza de que não ocorrerá mais o lançamento. 
        /// </summary>
        [Description("Cancelado")]
        Canceled = 5,
        /// <summary>
        /// Serve para avisar ao fluxo de caixa que o lançamento não poderá ser considerado ainda por ter alguma coisa errada.
        /// </summary>
        [Description("Inválido")]
        Invalid = 6

    }
}