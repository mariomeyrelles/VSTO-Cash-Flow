using System;
using System.IO;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Domain.Services;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.Excel2010.WorksheetLogic;
using Ninject;

namespace ModernCashFlow.Excel2010
{

    /// <summary>
    /// Esta classe configura o Ninject para fazer a inje��o de depend�ncia neste projeto.
    /// </summary>
    public static class NinjectContainer
    {
        private static IKernel _kernel;

        /// <summary>
        /// O kernel � o container de inje��o de depend�ncia usado para simplicar a constru��o dos objetos neste projeto.
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
        /// Configura a inje��o de depend�ncia para este projeto.
        /// </summary>
        private static void Start()
        {
            ////ler o arquivo de configura��o em busca do endere�o do modelo de dados e conex�o com o banco.
            //var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.Config");

            ////ler o xml do app.config e coloc�-lo em um XElement.
            //var configContent = XElement.Load(configFile);

            //var cstr = configContent.Element("connectionStrings").
            //                        Element("add").
            //                        Attribute("connectionString").Value;

            //cria uma inst�ncia do kernel do Ninject.
            _kernel = new StandardKernel();

            _kernel.Bind<ExpenseWorksheet>().ToSelf().InSingletonScope();
            _kernel.Bind<ExpenseWorksheet.Events>().ToSelf().InSingletonScope();
            _kernel.Bind<ExpenseWorksheet.ContextMenus>().ToSelf().InSingletonScope();
            _kernel.Bind<IncomeWorksheet>().ToSelf().InSingletonScope();
            _kernel.Bind<IncomeWorksheet.Events>().ToSelf().InSingletonScope();
            _kernel.Bind<IncomeWorksheet.ContextMenus>().ToSelf().InSingletonScope();
            _kernel.Bind<AccountWorksheet>().ToSelf().InSingletonScope();

            
            _kernel.Bind<BaseController<Expense>>().To<ExpenseController>().InSingletonScope();
            _kernel.Bind<BaseController<Income>>().To<IncomeController>().InSingletonScope();
            _kernel.Bind<BaseController<Account>>().To<AccountController>().InSingletonScope();

            _kernel.Bind<CommandManager>().ToSelf().InSingletonScope();
            
            //j� tornar os gerenciadores de planilha dispon�veis ao iniciar a aplica��o.
            //_kernel.Get<ExpenseWorksheet>();


            //servi�os de dom�nio
            _kernel.Bind<ExpenseStatusService>().ToSelf().InSingletonScope();

        }

    }
}
