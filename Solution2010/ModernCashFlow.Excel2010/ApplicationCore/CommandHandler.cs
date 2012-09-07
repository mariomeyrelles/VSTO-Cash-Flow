using ModernCashFlow.Excel2010.Commands;
using Ninject;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    public class CommandHandler
    {

        internal static void Send<T>(CommandArgs commandArgs = null) where T : ICommand
        {
            NinjectContainer.Kernel.Get<T>().Execute(commandArgs);
        }
    }
}