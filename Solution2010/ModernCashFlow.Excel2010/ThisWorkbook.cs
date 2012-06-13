using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using ModernCashFlow.Excel2010.Commands;
using ModernCashFlow.Excel2010.WorksheetLogic;
using ModernCashFlow.Globalization.Resources;
using ModernCashFlow.WpfControls;
using Ninject;
using Application = System.Windows.Application;
using Excel = Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;
using Office = Microsoft.Office.Core;
using ModernCashFlow.Excel2010.ApplicationCore;
using Action = Microsoft.Office.Interop.Excel.Action;

namespace ModernCashFlow.Excel2010
{
    public partial class ThisWorkbook
    {
        //private Application _wpfApp;
        private static int _sheeetCount;
        public static event EventHandler WorksheetsLoaded;

        private void ThisWorkbookStartup(object sender, System.EventArgs e)
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
            //    new Uri("ModernCashFlow.WpfControls;component/Resources/CustomResources.xaml", UriKind.RelativeOrAbsolute))
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

            WorksheetsLoaded += ThisWorkbookWorksheetsLoaded;
        }

        private static void OnWorksheetsLoaded()
        {
            if (WorksheetsLoaded != null)
            {
                WorksheetsLoaded(null, null);
            }
        }

        private void ThisWorkbookWorksheetsLoaded(object sender, EventArgs e)
        {
            CommandHandler.Send<InitializeBasicDependenciesCommand>(new CommandArgs());
            CommandHandler.Send<InitializeMainWorkooksCommand>(new CommandArgs());
            CommandHandler.Send<InitializeBusinessRulesCommand>(new CommandArgs());
            CommandHandler.Send<ConfigureSidePanelCommand>(new SidePanelCommandArgs {WpfControl = new SaidaInspector()});
        }


        private void ThisWorkbookShutdown(object sender, System.EventArgs e)
        {
            //todo: finalizar a instância do engine do WPF ?.
            //_wpfApp.Shutdown();
        }

        private void ThisWorkbookBeforeSave(bool saveAsUi, ref bool cancel)
        {
            //todo: rever processos do before save.
            var eventHandlers = NinjectContainer.Kernel.Get<ExpenseWorksheet.Events>();
            eventHandlers.BeforeSave(saveAsUi,ref cancel);
        }

        private void ThisWorkbookBeforeClose(ref bool cancel)
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
            this.Startup += (ThisWorkbookStartup);
            this.Shutdown += (ThisWorkbookShutdown);
            this.BeforeSave += (ThisWorkbookBeforeSave);
            this.BeforeClose += (ThisWorkbookBeforeClose);

        }

       
        #endregion

    }
}
