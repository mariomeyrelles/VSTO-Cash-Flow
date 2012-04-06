using System;
using System.ComponentModel;

namespace ModernCashFlow.Domain.Entities
{
    /// <summary>
    /// Status geral de um determinado lan�amento. De acordo com o status � poss�vel decidir de j� � poss�vel considerar ou n�o
    /// este lan�amento para montar o fluxo de caixa.
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Status que indica que n�o se sabe ainda o status do lan�amento.
        /// </summary>
        [Description("-")]
        Unknown = 0,
        /// <summary>
        /// Quando o lan�amento est� previsto, espera-se que a data do lan�amento aconte�a na data prevista.
        /// </summary>
        [Description("Previsto")]
        Scheduled = 1,
        /// <summary>
        /// O lan�amento � considerando pendente quando a data de vencimento j� passou e n�o foi dado baixa no lan�amento.
        /// </summary>
        [Description("Pendente")]
        Pending = 2,
        /// <summary>
        /// O lan�amento est� OK quando o us�rio confirma o lan�amento adequadamente.
        /// </summary>
        [Description("OK")]
        OK = 3,
        /// <summary>
        /// O lan�amento est� suspenso quando o usu�rio decide n�o lan�ar mais este valor. Para efeitos de fluxo de caixa
        /// este lan�amento n�o existir� mais.
        /// </summary>
        [Description("Suspenso")]
        Suspended = 4,
        /// <summary>
        /// O lan�amento � cancelado quando se tem a certeza de que n�o ocorrer� mais o lan�amento. 
        /// </summary>
        [Description("Cancelado")]
        Canceled = 5,
        /// <summary>
        /// Serve para avisar ao fluxo de caixa que o lan�amento n�o poder� ser considerado ainda por ter alguma coisa errada.
        /// </summary>
        [Description("Inv�lido")]
        Invalid = 6

    }
}