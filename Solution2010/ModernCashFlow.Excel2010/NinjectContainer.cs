using System;
using System.Diagnostics;
using System.IO;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.ApplicationCore.Factories;
using ModernCashFlow.Excel2010.Commands;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;
using Ninject.Extensions.Factory;

namespace ModernCashFlow.Excel2010
{

    /// <summary>
    /// Esta classe configura o Ninject para fazer a inje��o de depend�ncia neste projeto.
    /// </summary>
    public static class NinjectContainer
    {
        private static IKernel _kernel;

        public static object SyncLock = new object();

        /// <summary>
        /// O kernel � o container de inje��o de depend�ncia usado para simplicar a constru��o dos objetos neste projeto.
        /// </summary>
        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    lock (SyncLock)
                    {
                        Start();
                    }
                }
                return _kernel;
            }
        }


        /// <summary>
        /// Configures the Dependency Injection bindings for the project.
        /// </summary>
        private static void Start()
        {
            #region After deploy code

            ////ler o arquivo de configura��o em busca do endere�o do modelo de dados e conex�o com o banco.
            //var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.Config");

            ////ler o xml do app.config e coloc�-lo em um XElement.
            //var configContent = XElement.Load(configFile);

            //var cstr = configContent.Element("connectionStrings").
            //                        Element("add").
            //                        Attribute("connectionString").Value;

            #endregion

            //cria uma inst�ncia do kernel do Ninject.
            var kernel = new StandardKernel();
           
            //worksheet related stuff - seems to be ok to be singleton
            kernel.Bind<ExpenseWorksheet>().ToSelf().InSingletonScope();
            kernel.Bind<ExpenseWorksheet.Events>().ToSelf().InSingletonScope();
            kernel.Bind<ExpenseWorksheet.ContextMenus>().ToSelf().InSingletonScope();
            kernel.Bind<IncomeWorksheet>().ToSelf().InSingletonScope();
            kernel.Bind<IncomeWorksheet.Events>().ToSelf().InSingletonScope();
            kernel.Bind<IncomeWorksheet.ContextMenus>().ToSelf().InSingletonScope();
            kernel.Bind<AccountWorksheet>().ToSelf().InSingletonScope();

            //the controllers in this case maintain state and should be singleton.
            kernel.Bind<BaseController<Expense>>().To<ExpenseController>().InSingletonScope();
            kernel.Bind<BaseController<Income>>().To<IncomeController>().InSingletonScope();
            kernel.Bind<BaseController<Account>>().To<AccountController>().InSingletonScope();

            kernel.Bind<CommandManager>().ToSelf().InSingletonScope();
            //singleton commands
            kernel.Bind<ConfigureSidePanelCommand>().ToSelf().InSingletonScope();
            //non-singleton commands
            kernel.Bind<ICommand>().To<InitializeBasicBusinessDependenciesCommand>();
            kernel.Bind<ICommand>().To<InitializeMainWorksheetsCommand>();
            kernel.Bind<ICommand>().To<InitializeBusinessRulesCommand>();


            //servi�os de dom�nio
            kernel.Bind<ExpenseStatusService>().ToSelf().InSingletonScope();


            //factories
            kernel.Bind<IExpenseWorksheetFactory>().ToFactory();
            kernel.Bind<IIncomeWorksheetFactory>().ToFactory();

            _kernel = kernel;
        }

        private static void OnAsyncCallback(IAsyncResult ar)
        {
            _kernel = ar.AsyncState as IKernel;
        }
    }
}