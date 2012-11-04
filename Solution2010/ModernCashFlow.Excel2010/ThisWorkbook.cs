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
        private Application _wpfApp;
        private static int _sheeetCount;
        public static event EventHandler WorksheetsLoaded;

        private void ThisWorkbookStartup(object sender, System.EventArgs e)
        {
            //all initialization code should be placed here.
            _sheeetCount = this.Sheets.Count;

            //set culture.
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pt-br");

            //impedir usuário de arrastar células 
            ThisApplication.CellDragAndDrop = false;

            //Start WPF in another thread.
            var wpfInit = new System.Action(InitializeWpfEngine);

            //wpfInit.BeginInvoke(null, null);
            wpfInit();

            WorksheetsLoaded += ThisWorkbookWorksheetsLoaded;
        }

        private void InitializeWpfEngine()
        {
            // Create a WPF application 
            _wpfApp = new System.Windows.Application();
            

            // Load the ressources
            var resources = System.Windows.Application.LoadComponent(
                new Uri("ModernCashFlow.WpfControls;component/Resources/CustomResources.xaml",
                        UriKind.RelativeOrAbsolute))
                            as System.Windows.ResourceDictionary;

            //Load Telerik WPF theme: Summer
            var uri1 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.xaml", UriKind.RelativeOrAbsolute);
            var uri2 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/System.Windows.xaml", UriKind.RelativeOrAbsolute);
            var uri3 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.Input.xaml", UriKind.RelativeOrAbsolute);
            var uri4 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.Navigation.xaml", UriKind.RelativeOrAbsolute);
            //var uri5 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.Chart.xaml", UriKind.RelativeOrAbsolute);
            //var uri6 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.Data.xaml", UriKind.RelativeOrAbsolute);
            //var uri7 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.DataVisualization.xaml", UriKind.RelativeOrAbsolute);
            //var uri8 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.Expressions.xaml", UriKind.RelativeOrAbsolute);
            var uri9 = new Uri("/Telerik.Windows.Themes.Summer;component/Themes/Telerik.Windows.Controls.Gridview.xaml", UriKind.RelativeOrAbsolute);
            var r1 = System.Windows.Application.LoadComponent(uri1) as System.Windows.ResourceDictionary;
            var r2 = System.Windows.Application.LoadComponent(uri2) as System.Windows.ResourceDictionary;
            var r3 = System.Windows.Application.LoadComponent(uri3) as System.Windows.ResourceDictionary;
            var r4 = System.Windows.Application.LoadComponent(uri4) as System.Windows.ResourceDictionary;
            //var r5 = System.Windows.Application.LoadComponent(uri5) as System.Windows.ResourceDictionary;
            //var r6 = System.Windows.Application.LoadComponent(uri6) as System.Windows.ResourceDictionary;
            //var r7 = System.Windows.Application.LoadComponent(uri7) as System.Windows.ResourceDictionary;
            //var r8 = System.Windows.Application.LoadComponent(uri8) as System.Windows.ResourceDictionary;
            var r9 = System.Windows.Application.LoadComponent(uri9) as System.Windows.ResourceDictionary;
            
            // Merge it on application level
            _wpfApp.Resources.MergedDictionaries.Add(resources);
            _wpfApp.Resources.MergedDictionaries.Add(r1);
            _wpfApp.Resources.MergedDictionaries.Add(r2);
            _wpfApp.Resources.MergedDictionaries.Add(r3);
            _wpfApp.Resources.MergedDictionaries.Add(r4);
            //_wpfApp.Resources.MergedDictionaries.Add(r5);
            //_wpfApp.Resources.MergedDictionaries.Add(r6);
            //_wpfApp.Resources.MergedDictionaries.Add(r7);
            //_wpfApp.Resources.MergedDictionaries.Add(r8);
            _wpfApp.Resources.MergedDictionaries.Add(r9);
            

            // //dizer ao WPF que é preciso aceitar a linguagem padrão do sistema.
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));
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
            CommandHandler.Run<InitializeBasicBusinessDependenciesCommand>();
            CommandHandler.Run<InitializeMainWorksheetsCommand>();
            CommandHandler.Run<InitializeBusinessRulesCommand>();
            
        }


        private void ThisWorkbookShutdown(object sender, System.EventArgs e)
        {
            //_wpfApp.Shutdown();
        }

        private void ThisWorkbookBeforeSave(bool saveAsUi, ref bool cancel)
        {
            //todo: rever processos do before save.
            var eventHandlers = NinjectContainer.Kernel.Get<ExpenseWorksheet.Events>();
            eventHandlers.BeforeSave(saveAsUi, ref cancel);
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
