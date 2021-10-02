using Collect.Models;
using ScottPlot;
using ScottPlot.Plottable;
using Stylet;
using System;
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
            set { SetAndNotify(ref _allowConnect, value); }
        }
        private bool _allowDisconnect;
        public bool AllowDisconnect
        {
            get { return _allowDisconnect; }
            set { SetAndNotify(ref _allowDisconnect, value); }
        }
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
        private bool _isCollecting;
        public bool IsCollecting
        {
            get { return _isCollecting; }
            set { SetAndNotify(ref _isCollecting, value); }
        }
        private bool _isNotCollecting;
        public bool IsNotCollecting
        {
            get { return _isNotCollecting; }
            set { SetAndNotify(ref _isNotCollecting, value); }
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
            XLocked = false;
            YLocked = false;
            // OPC
            AllowConnect = true;
            AllowDisconnect = !AllowConnect;
            GroupUpdateRate = 500;
            Server = null;
            // Tags and Data
            IsCollecting = false;
            IsNotCollecting = !IsCollecting;
            Tags = new BindableCollection<Tag>();
            Tags.CollectionChanged += Tags_CollectionChanged;

            // Testing code
            GenerateSampleTags();
            PlotAllTags(100);
            WpfPlot.Refresh();
        }
        #endregion

        #region Actions
        // OPC
        public async Task Connect()
        {
            AllowConnect = false;
            AllowDisconnect = !AllowConnect;
        }

        public async Task DataCollectionEnable()
        {
            IsCollecting = true;
            IsNotCollecting = !IsCollecting;
        }

        public async Task DataCollectionDisable()
        {
            IsCollecting = false;
            IsNotCollecting = !IsCollecting;
        }

        public async Task Disconnect()
        {
            AllowConnect = true;
            AllowDisconnect = !AllowConnect;
        }

        // Tags
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

        public async Task ShowTagDialog()
        {
            var dialogVm = this.dialogFactory.CreateTagDialog();
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
        public void GenerateSampleTags()
        {
            Tags.Add(new Tag("test1", Color.Green, 0, 0));
            Tags[0].Data.Add(new double[] { 0, 0 });
            Tags[0].Data.Add(new double[] { 1, 10 });
            Tags[0].Data.Add(new double[] { 2, 2 });
            Tags[0].Data.Add(new double[] { 3, 3 });
            Tags[0].Data.Add(new double[] { 4, 3 });
            Tags[0].Data.Add(new double[] { 7, 7 });

            Tags.Add(new Tag("test2", Color.Red, 0, 0));
            Tags[1].Data.Add(new double[] { 0, 9 });
            Tags[1].Data.Add(new double[] { 1, 8 });
            Tags[1].Data.Add(new double[] { 2, 9 });
            Tags[1].Data.Add(new double[] { 3, 2 });
            Tags[1].Data.Add(new double[] { 4, 2 });
            Tags[1].Data.Add(new double[] { 7, 7 });

            Tags.Add(new Tag("test3", Color.Black, 0, 0));
            Tags[2].Data.Add(new double[] { 0, 3 });
            Tags[2].Data.Add(new double[] { 1, 3 });
            Tags[2].Data.Add(new double[] { 2, 4 });
            Tags[2].Data.Add(new double[] { 3, 5 });
            Tags[2].Data.Add(new double[] { 4, 4 });
            Tags[2].Data.Add(new double[] { 7, 1 });
        }

        public void PlotAllTags(int timeSpanSeconds)
        {
            // Clear the plot
            WpfPlot.Plot.Clear();

            // Add each tag's plottable back
            if (Tags != null)
            { 
                foreach (Tag tag in Tags)
                {
                    var splt = new SignalPlotXY();
                    splt.Xs = tag.Data.Select(x => x[0]).ToArray();
                    splt.Ys = tag.Data.Select(x => x[1]).ToArray();
                    splt.Color = tag.TagBase.TraceColor;
                    splt.MarkerSize = 0;
                    splt.MaxRenderIndex = splt.Xs.Length - 1;
                    splt.MinRenderIndex = 0;
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
        TagManagerDialogViewModel CreateTagDialog();
    }
}
