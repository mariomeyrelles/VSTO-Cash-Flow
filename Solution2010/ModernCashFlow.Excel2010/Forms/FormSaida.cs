using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModernCashFlow.Domain.Entities;
using ModernCashFlow.Excel2010.ApplicationCore;
using ModernCashFlow.WpfTests;
using Ninject;

namespace ModernCashFlow.Excel2010.Forms
{
    public partial class FormExpense : Form
    {
        
        public FormExpense()
        {
            InitializeComponent();
            //todo: obviamente isso é só um exemplo
            //todo: edição de um item específico do grid do excel




            _wpfControl = new UserControl1();
            this.elementHost1.Child = _wpfControl;

            _wpfControl.Save += WpfControlSave;
            _wpfControl.Next += WpfControlNext;
            _wpfControl.Previous += WpfControlPrevious;

            this.Closing += FormSaidaClosing;

            _controller = NinjectContainer.Kernel.Get<BaseController<Expense>>();
        }

        public Expense Model
        {
            set
            {
                _wpfControl.ModelData = _activeModel = value;
            }
        }

        private void WpfControlPrevious(object sender, EventArgs e)
        {
            var prevEntity = (from x in _controller.DataManager.OrderByDescending(x=>x.Rownum)
                              where x.Rownum < _activeModel.Rownum
                              select x).FirstOrDefault();

            if (prevEntity != null)
            {
                Model = prevEntity;
            }
        }

        private void WpfControlNext(object sender, EventArgs e)
        {
            var nextEntity = (from x in _controller.DataManager
                             where x.Rownum > _activeModel.Rownum
                             select x).FirstOrDefault();

            if (nextEntity != null)
            {
                Model = nextEntity;
            }
        }

        private void FormSaidaClosing(object sender, CancelEventArgs e)
        {
            _controller.RefreshAllLocalData();
        }
        
        private void WpfControlSave(object sender, EventArgs e)
        {
            _controller.AcceptData(_wpfControl.ModelData);
        }

        private readonly UserControl1 _wpfControl;
        private Expense _activeModel;
        private BaseController<Expense> _controller;

    }
}
