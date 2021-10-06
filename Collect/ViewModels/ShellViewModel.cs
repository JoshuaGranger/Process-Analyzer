﻿using Collect.Models;
using ScottPlot;
using ScottPlot.Plottable;
using Stylet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Pages
{
    public class ShellViewModel : Screen
    {
        #region Properties
        // Stylet
        private IDialogFactory dialogFactory;
        private IWindowManager windowManager;
        // Plot Properties
        private bool _autoScale;
        public bool AutoScale
        {
            get { return _autoScale; }
            set { SetAndNotify(ref _autoScale, value); }
        }
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
            set { SetAndNotify(ref _allowConnect, value); AllowDisconnect = !AllowConnect; }
        }
        private bool _allowDisconnect;
        public bool AllowDisconnect
        {
            get { return _allowDisconnect; }
            set { SetAndNotify(ref _allowDisconnect, value); }
        }
        private List<Data> Data;
        private int _groupUpdateRate;
        public int GroupUpdateRate
        {
            get { return _groupUpdateRate; }
            set { SetAndNotify(ref _groupUpdateRate, value); }
        }
        private OpcDaServer _server;
        public OpcDaServer Server
        {
            get { return _server; }
            set { SetAndNotify(ref _server, value); UpdateServerStatus(); }
        }
        private string _serverStatus;               // Set by Server
        public string ServerStatus
        {
            get { return _serverStatus; }
            set { SetAndNotify(ref _serverStatus, value); }
        }
        // Tags and Data
        public int[] CustomColors;
        private bool _isCollecting;
        public bool IsCollecting
        {
            get { return _isCollecting; }
            set { SetAndNotify(ref _isCollecting, value); IsNotCollecting = !IsCollecting; }
        }
        private bool _isNotCollecting;
        public bool IsNotCollecting
        {
            get { return _isNotCollecting; }
            set { SetAndNotify(ref _isNotCollecting, value); }
        }
        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set { SetAndNotify(ref _selectedTag, value); }
        }	
        private BindableCollection<Tag> _tags;
        public BindableCollection<Tag> Tags
        {
            get { return _tags; }
            set { SetAndNotify(ref _tags, value); }
        }

        // Commands Allowed
        private bool _canExportTags;
        public bool CanExportTags
        {
            get { return _canExportTags; }
            set { SetAndNotify(ref _canExportTags, value); }
        }
        #endregion

        #region Constructor
        public ShellViewModel(IWindowManager windowManager, IDialogFactory dialogFactory)
        {
            // Property initialization
            // Stylet
            this.windowManager = windowManager;
            this.dialogFactory = dialogFactory;
            // Plot
            AutoScale = true;
            WpfPlot = new WpfPlot();
            XLocked = true;
            YLocked = true;
            // OPC
            AllowConnect = true;
            GroupUpdateRate = 500;
            Server = null;
            // Tags and Data
            IsCollecting = false;
            Data = new List<Data>();
            Tags = new BindableCollection<Tag>();
            Tags.Add(new Tag(new OpcDaItemDefinition(), "", Color.Black));
            Tags.CollectionChanged += Tags_CollectionChanged;

            // Testing code
            PlotAllTags(120);
            WpfPlot.Refresh();
        }
        #endregion

        #region Actions
        // OPC
        public async Task Connect()
        {
            AllowConnect = false;
        }

        public async Task DataCollectionEnable()
        {
            IsCollecting = true;
        }

        public async Task DataCollectionDisable()
        {
            IsCollecting = false;
        }

        public async Task Disconnect()
        {
            AllowConnect = true;
        }

        // Tags
        public async Task ColorChange()
        {
            // TODO: make the color changing mechanism more WPF/MVVM-like
            var tag = SelectedTag;
            
            var colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.SolidColorOnly = true;
            colorDialog.FullOpen = true;

            if (CustomColors != null)
                colorDialog.CustomColors = CustomColors;

            var dialogResult = colorDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                tag.TraceColor = colorDialog.Color;
            }

            CustomColors = colorDialog.CustomColors;
        }

        public async Task ImportTags()
        {

        }

        public async Task ExportTags()
        {

        }

        // Plot
        public async Task LockXAxis()
        {
            WpfPlot.Plot.XAxis.LockLimits(XLocked = !XLocked);
        }

        public async Task LockYAxis()
        {
            WpfPlot.Plot.YAxis.LockLimits(YLocked = !YLocked);
        }

        public async Task ScaleAllTime()
        {
            WpfPlot.Plot.AxisAutoY();
            WpfPlot.Refresh();
        }

        public async Task ScaleToWindow()
        {
            double min = WpfPlot.Plot.XAxis.Dims.Min;
            double max = WpfPlot.Plot.XAxis.Dims.Max;

            foreach (ScottPlot.Plottable.SignalPlotXY p in WpfPlot.Plot.GetSettings().Plottables)
            {
                
            }

            WpfPlot.Plot.AxisAutoY();
            WpfPlot.Refresh();
        }

        public async Task SetAutoScale()
        {
            AutoScale = !AutoScale;
        }

        // Dialog boxes
        public async Task ShowAboutDialog()
        {
            var dialogVm = this.dialogFactory.CreateAboutDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
        }

        public async Task ShowDatalogDialog()
        {
            var dialogVm = this.dialogFactory.CreateDatalogDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
        }

        public async Task ShowRateDialog()
        {
            var dialogVm = this.dialogFactory.CreateRateDialog();
            dialogVm.GroupUpdateRate = this.GroupUpdateRate.ToString();
            var result = this.windowManager.ShowDialog(dialogVm);
            if (result.GetValueOrDefault())
                GroupUpdateRate = int.Parse(dialogVm.GroupUpdateRate);
        }

        public async Task ShowServerDialog()
        {
            var dialogVm = this.dialogFactory.CreateServerDialog();
            var result = this.windowManager.ShowDialog(dialogVm);
            if (result.GetValueOrDefault())
                Server = dialogVm.Server;
            else
                Server = null;
        }

        public async Task ShowTagManagerDialog()
        {
            var dialogVm = this.dialogFactory.CreateTagManagerDialog();
            dialogVm.Tags = Tags;
            dialogVm.CustomColors = CustomColors;
            var result = this.windowManager.ShowDialog(dialogVm);
            //if (result.GetValueOrDefault())
            //    Server = dialogVm.Server;
            //else
            //    Server = null;
        }
        #endregion

        #region Other Methods
        // OPC
        public void UpdateServerStatus()
        {
            if (Server == null)
                ServerStatus = "Disconnected";
            else
                ServerStatus = "Connected to " + Server.ServerDescription;
        }

        // Tags
        private void Tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExportTags = Tags.Count > 0;
        }

        // Plot
        public void PlotAllTags(int timeSpanSeconds)
        {
            // Clear the plot
            WpfPlot.Plot.Clear();

            // Figure out the plot times
            var startTime = DateTime.Now.AddSeconds((-1) * timeSpanSeconds).ToOADate();
            var endTime = DateTime.Now.ToOADate();

            // Select tags that are requested to be shown
            var activeTags = Tags.Where(x => x.ShowTrace == true).ToList();

            foreach (Tag tag in activeTags)
            {
                // Check to see if there is any data for this tag
                Data dataSet = Data.Where(x => x.TagId == tag.TagId).ToList().FirstOrDefault();

                // Data exists
                if (dataSet != default(IEnumerable<Data>))
                {
                    var splt = new SignalPlotXY();

                    // Filter data points to only those in the shown timespan
                    var filteredData = dataSet.Points.Where(x => x[0] > startTime).ToList();

                    splt.Xs = filteredData.Select(x => x[0]).ToArray();
                    splt.Ys = filteredData.Select(x => x[1]).ToArray();
                    splt.Color = tag.TraceColor;
                    splt.MarkerSize = 0;
                    splt.MaxRenderIndex = (int)endTime;
                    splt.MinRenderIndex = (int)startTime;
                    WpfPlot.Plot.Add(splt);
                }
            }
        }
        #endregion
    }

    public interface IDialogFactory
    {
        AboutDialogViewModel CreateAboutDialog();
        DatalogDialogViewModel CreateDatalogDialog();
        RateDialogViewModel CreateRateDialog();
        ServerDialogViewModel CreateServerDialog();
        TagManagerDialogViewModel CreateTagManagerDialog();
    }
}