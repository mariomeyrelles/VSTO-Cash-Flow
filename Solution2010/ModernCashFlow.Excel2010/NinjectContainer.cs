using System;
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
    /// Esta classe configura o Ninject para fazer a injeção de dependência neste projeto.
    /// </summary>
    public static class NinjectContainer
    {
        private static IKernel _kernel;

        /// <summary>
        /// O kernel é o container de injeção de dependência usado para simplicar a construção dos objetos neste projeto.
        /// </summary>
        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    Start();
                }
                return _kernel;
            }
        }


        /// <summary>
        /// Configura a injeção de dependência para este projeto.
        /// </summary>
        private static void Start()
        {
            #region After deploy code

////ler o arquivo de configuração em busca do endereço do modelo de dados e conexão com o banco.
            //var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.Config");

            ////ler o xml do app.config e colocá-lo em um XElement.
            //var configContent = XElement.Load(configFile);

            //var cstr = configContent.Element("connectionStrings").
            //                        Element("add").
            //                        Attribute("connectionString").Value;

            #endregion


            //cria uma instância do kernel do Ninject.
            _kernel = new StandardKernel();

            //worksheet related stuff - seems to be ok to be singleton
            _kernel.Bind<ExpenseWorksheet>().ToSelf().InSingletonScope();
            _kernel.Bind<ExpenseWorksheet.Events>().ToSelf().InSingletonScope();
            _kernel.Bind<ExpenseWorksheet.ContextMenus>().ToSelf().InSingletonScope();
            _kernel.Bind<IncomeWorksheet>().ToSelf().InSingletonScope();
            _kernel.Bind<IncomeWorksheet.Events>().ToSelf().InSingletonScope();
            _kernel.Bind<IncomeWorksheet.ContextMenus>().ToSelf().InSingletonScope();
            _kernel.Bind<AccountWorksheet>().ToSelf().InSingletonScope();
            
            //the controllers in this case maintain state and should be singleton.
            _kernel.Bind<BaseController<Expense>>().To<ExpenseController>().InSingletonScope();
            _kernel.Bind<BaseController<Income>>().To<IncomeController>().InSingletonScope();
            _kernel.Bind<BaseController<Account>>().To<AccountController>().InSingletonScope();
            
            _kernel.Bind<CommandManager>().ToSelf().InSingletonScope();
            //singleton commands
            _kernel.Bind<ConfigureSidePanelCommand>().ToSelf().InSingletonScope();
            //non-singleton commands
            _kernel.Bind<ICommand>().To<InitializeBasicBusinessDependenciesCommand>();
            _kernel.Bind<ICommand>().To<InitializeMainWorksheetsCommand>();
            _kernel.Bind<ICommand>().To<InitializeBusinessRulesCommand>();

            
            //serviços de domínio
            _kernel.Bind<ExpenseStatusService>().ToSelf().InSingletonScope();

           
            //factories
            _kernel.Bind<IExpenseWorksheetFactory>().ToFactory();
            _kernel.Bind<IIncomeWorksheetFactory>().ToFactory();
        }
    }
}