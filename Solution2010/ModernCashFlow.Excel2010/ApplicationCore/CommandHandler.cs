using System;
using ModernCashFlow.Excel2010.Commands;
using Ninject;

namespace ModernCashFlow.Excel2010.ApplicationCore
{
    public class CommandHandler
    {

        internal static void Run<T>(CommandArgs commandArgs = null) where T : ICommand
        {
            NinjectContainer.Kernel.Get<T>().Execute(commandArgs);
        }


        internal static void RunAsync<T>(CommandArgs commandArgs = null) where T : ICommand
        {
            Action command = () => NinjectContainer.Kernel.Get<T>().Execute(commandArgs);
            command.BeginInvoke(null, null);
        }



    }
}