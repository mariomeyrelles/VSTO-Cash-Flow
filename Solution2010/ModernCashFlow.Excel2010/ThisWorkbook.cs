using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using ModernCashFlow.Excel2010.WorksheetLogic;
using ModernCashFlow.Globalization.Resources;
using Ninject;
using Application = System.Windows.Application;
using Excel = Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;
using Office = Microsoft.Office.Core;
using ModernCashFlow.Excel2010.ApplicationCore;

namespace ModernCashFlow.Excel2010
{
    public partial class ThisWorkbook
    {
        private Application _wpfApp;
        private static int _sheeetCount;
        public static event EventHandler WorksheetsLoaded;

        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            _sheeetCount = this.Sheets.Count;
            var kernel = NinjectContainer.Kernel;
            if (kernel == null)
            {
                throw new ApplicationException(Lang.Failed_to_load_Ninject);
            }

            //todo: verificar como setar a culture de forma mais legal.
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt-br");

            //impedir usuário de arrastar células 
            ThisApplication.CellDragAndDrop = false;
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            //iniciando WPF para conseguir expor recursos para os user controls acessarem.

            // Create a WPF application 
            //_wpfApp = new System.Windows.Application();


            // Load the ressources
            //var resources = System.Windows.Application.LoadComponent(
            //    new Uri("ModernCashFlow.WpfTests;component/Resources/CustomResources.xaml", UriKind.RelativeOrAbsolute))
            //                as System.Windows.ResourceDictionary;

            //// Recursos visuais do Reuxables
           // //var resource2 = System.Windows.Application.LoadComponent(
           // //    new Uri("/ReuxablesLegacy;component/edge.xaml", UriKind.RelativeOrAbsolute))
           // //                as System.Windows.ResourceDictionary;

           // // Merge it on application level
            //_wpfApp.Resources.MergedDictionaries.Add(resources);
           // _wpfApp.Resources.MergedDictionaries.Add(resource2);

           // //dizer ao WPF que é preciso aceitar a linguagem padrão do sistema.
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                                            XmlLanguage.GetLanguage(
                                            CultureInfo.CurrentCulture.IetfLanguageTag)));

            WorksheetsLoaded += new EventHandler(ThisWorkbook_WorksheetsLoaded);
        }

        private static void OnWorksheetsLoaded()
        {
            if (WorksheetsLoaded != null)
            {
                WorksheetsLoaded(null, null);
            }
        }

        private void ThisWorkbook_WorksheetsLoaded(object sender, EventArgs e)
        {
            //note: colocar coisas genéricas do startup da app

            //initialize accounts
            var commandManager = NinjectContainer.Kernel.Get<CommandManager>();
            NinjectContainer.Kernel.Get<AccountWorksheet>().Start();
            commandManager.LoadAccountData();

            
            //initalize other worksheet helpers
            NinjectContainer.Kernel.Get<IncomeWorksheet>().Start();
            NinjectContainer.Kernel.Get<ExpenseWorksheet>().Start();
           
            
            //business rules intialization code

            commandManager.LoadAllTransactions();
            //commandManager.ConvertTodayPaymentsToPending();
            //commandManager.WriteAllTransactionsToWorsheets();
            //commandManager.ShowSplashWindow();
            commandManager.ConfigureSidePanel();
        }

        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
            //todo: finalizar a instância do engine do WPF ?.
            //_wpfApp.Shutdown();
            
            ThisApplication.CellDragAndDrop = true;

        }

        private void ThisWorkbook_BeforeSave(bool SaveAsUI, ref bool Cancel)
        {
            //todo: rever processos do before save.
            var eventHandlers = NinjectContainer.Kernel.Get<ExpenseWorksheet.Events>();
            eventHandlers.BeforeSave(SaveAsUI,ref Cancel);
        }

        private void ThisWorkbook_BeforeClose(ref bool cancel)
        {
              ThisApplication.CellDragAndDrop = true;
        }


        public static void NotifySheetLoaded(WorksheetBase sheet)
        {
            _sheeetCount--;

            if (_sheeetCount == 0)
            {
                OnWorksheetsLoaded();
            }

        }




        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += (ThisWorkbook_Startup);
            this.Shutdown += (ThisWorkbook_Shutdown);
            this.BeforeSave += (ThisWorkbook_BeforeSave);
            this.BeforeClose += (ThisWorkbook_BeforeClose);

        }

       
        #endregion

    }
}
