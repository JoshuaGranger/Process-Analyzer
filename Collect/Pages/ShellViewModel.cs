﻿using ScottPlot;
using Stylet;
using System;

namespace Collect.Pages
{
    public class ShellViewModel : Screen
    {
        #region Properties
        private IWindowManager windowManager;
        private IDialogFactory dialogFactory;

        // Plot Properties
        private WpfPlot _wpfPlot;
        public WpfPlot WpfPlot
        {
            get { return _wpfPlot; }
            set { SetAndNotify(ref _wpfPlot, value); }
        }
        private bool _xLocked;
        public bool XLocked
        {
            get { return _xLocked; }
            set { SetAndNotify(ref _xLocked, value); }
        }
        private bool _yLocked;
        public bool YLocked
        {
            get { return _yLocked; }
            set { SetAndNotify(ref _yLocked, value); }
        }

        // OPC Properties
        private bool _allowConnect;
        public bool AllowConnect
        {
            get { return _allowConnect; }
            set { SetAndNotify(ref _allowConnect, value); }
        }
        private bool _allowDisconnect;
        public bool AllowDisconnect
        {
            get { return _allowDisconnect; }
            set { SetAndNotify(ref _allowDisconnect, value); }
        }
        #endregion

        #region Constructor
        public ShellViewModel(IWindowManager windowManager, IDialogFactory dialogFactory)
        {
            this.windowManager = windowManager;
            this.dialogFactory = dialogFactory;
            WpfPlot = new WpfPlot();
            WpfPlot.Plot.AddScatter(new double[] { 0, 1, 2, 3 }, new double[] { 0, 10, 2, 3 });
            WpfPlot.Refresh();
            XLocked = false;
            _yLocked = false;
            AllowConnect = true;
            AllowDisconnect = !AllowConnect;
        }
        #endregion

        #region Actions
        public async System.Threading.Tasks.Task Connect()
        {
            AllowConnect = false;
            AllowDisconnect = !AllowConnect;
        }

        public async System.Threading.Tasks.Task Disconnect()
        {
            AllowConnect = true;
            AllowDisconnect = !AllowConnect;
        }

        public async System.Threading.Tasks.Task LockXAxis()
        {
            WpfPlot.Plot.XAxis.LockLimits(XLocked = !XLocked);
        }

        public async System.Threading.Tasks.Task LockYAxis()
        {
            WpfPlot.Plot.YAxis.LockLimits(YLocked = !YLocked);
        }

        public async System.Threading.Tasks.Task ShowServerDialog()
        {
            var dialogVm = this.dialogFactory.CreateServerDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
            Connect();
        }

        public async System.Threading.Tasks.Task ShowDatalogDialog()
        {
            var dialogVm = this.dialogFactory.CreateDatalogDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
        }

        public async System.Threading.Tasks.Task ShowAboutDialog()
        {
            var dialogVm = this.dialogFactory.CreateAboutDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
        }
        #endregion
    }

    public interface IDialogFactory
    {
        ServerDialogViewModel CreateServerDialog();
        DatalogDialogViewModel CreateDatalogDialog();
        AboutDialogViewModel CreateAboutDialog();
    }
}
