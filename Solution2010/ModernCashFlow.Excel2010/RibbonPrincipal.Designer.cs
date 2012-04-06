namespace ModernCashFlow.Excel2010
{
    partial class RibbonPrincipal : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonPrincipal()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.button1 = this.Factory.CreateRibbonButton();
            this.btnLerSaidas = this.Factory.CreateRibbonButton();
            this.btnEscreverSaidas = this.Factory.CreateRibbonButton();
            this.button2 = this.Factory.CreateRibbonButton();
            this.btnPendingPayments = this.Factory.CreateRibbonButton();
            this.button3 = this.Factory.CreateRibbonButton();
            this.button4 = this.Factory.CreateRibbonButton();
            this.toggleButton1 = this.Factory.CreateRibbonToggleButton();
            this.button5 = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "Modern Cash Flow";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.button1);
            this.group1.Items.Add(this.btnLerSaidas);
            this.group1.Items.Add(this.btnEscreverSaidas);
            this.group1.Items.Add(this.button2);
            this.group1.Items.Add(this.btnPendingPayments);
            this.group1.Items.Add(this.button3);
            this.group1.Items.Add(this.toggleButton1);
            this.group1.Items.Add(this.button4);
            this.group1.Items.Add(this.button5);
            this.group1.Label = "group1";
            this.group1.Name = "group1";
            // 
            // button1
            // 
            this.button1.Label = "Teste Janelas WPF";
            this.button1.Name = "button1";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // btnLerSaidas
            // 
            this.btnLerSaidas.Label = "Ler Saídas";
            this.btnLerSaidas.Name = "btnLerSaidas";
            this.btnLerSaidas.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.lerSaidas_Click);
            // 
            // btnEscreverSaidas
            // 
            this.btnEscreverSaidas.Label = "Escrever Saídas";
            this.btnEscreverSaidas.Name = "btnEscreverSaidas";
            this.btnEscreverSaidas.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnEscreverSaidas_Click);
            // 
            // button2
            // 
            this.button2.Label = "Testes";
            this.button2.Name = "button2";
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button2_Click);
            // 
            // btnPendingPayments
            // 
            this.btnPendingPayments.Label = "Pagamentos Pendentes";
            this.btnPendingPayments.Name = "btnPendingPayments";
            this.btnPendingPayments.ShowImage = true;
            this.btnPendingPayments.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPendingPayments_Click);
            // 
            // button3
            // 
            this.button3.Label = "Globalization";
            this.button3.Name = "button3";
            this.button3.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Label = "Ler Entradas";
            this.button4.Name = "button4";
            this.button4.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button4_Click);
            // 
            // toggleButton1
            // 
            this.toggleButton1.Label = "Drap and Drop";
            this.toggleButton1.Name = "toggleButton1";
            this.toggleButton1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.toggleButton1_Click);
            // 
            // button5
            // 
            this.button5.Label = "Escrever Entradas";
            this.button5.Name = "button5";
            this.button5.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button5_Click);
            // 
            // RibbonPrincipal
            // 
            this.Name = "RibbonPrincipal";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonPrincipal_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLerSaidas;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnEscreverSaidas;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPendingPayments;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button3;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button4;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button5;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonPrincipal RibbonPrincipal
        {
            get { return this.GetRibbon<RibbonPrincipal>(); }
        }
    }
}
