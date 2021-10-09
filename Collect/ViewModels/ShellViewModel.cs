using Collect.Models;
using Collect.Views;
using ScottPlot;
using ScottPlot.Plottable;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Views
{
    public class ShellViewModel : Screen
    {
        #region Properties
        // Stylet
        private IDialogFactory dialogFactory;
        private IWindowManager windowManager;
        // Plot Properties
        private bool _autoScroll;
        public bool AutoScroll
        {
            get { return _autoScroll; }
            set
            {
                SetAndNotify(ref _autoScroll, value);
                if (WpfPlot != null)
                {
                    if (AutoScroll)
                        WpfPlot.Plot.XAxis.LockLimits();
                    else
                        WpfPlot.Plot.XAxis.LockLimits(false);
                }
                //XLocked = AutoScroll ? true : XLocked;
            }
        }
        private WpfPlot _wpfPlot;
        public WpfPlot WpfPlot
        {
            get { return _wpfPlot; }
            set { SetAndNotify(ref _wpfPlot, value); }
        }
        //private bool _xLocked;
        //public bool XLocked
        //{
        //    get { return _xLocked; }
        //    set { SetAndNotify(ref _xLocked, value); }
        //}
        //private bool _yLocked;
        //public bool YLocked
        //{
        //    get { return _yLocked; }
        //    set { SetAndNotify(ref _yLocked, value); }
        //}
        private int plotTimeSpan;                   // Seconds
        public int PlotTimeSpan
        {
            get { return plotTimeSpan; }
            set { SetAndNotify(ref plotTimeSpan, value); }
        }	
        // OPC Properties
        private OpcDaServer _server;
        public OpcDaServer Server
        {
            get { return _server; }
            set
            {
                SetAndNotify(ref _server, value);

                // Server status string
                UpdateServerStatus();
            }
        }
        private OpcDaGroup _group;
        public OpcDaGroup Group
        {
            get { return _group; }
            set { SetAndNotify(ref _group, value); }
        }	
        private List<Data> Data;
        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                SetAndNotify(ref _isConnected, value);
                UpdateCanDataCollectionEnable();
            }
        }	
        private int _groupUpdateRate;
        public int GroupUpdateRate
        {
            get { return _groupUpdateRate; }
            set { SetAndNotify(ref _groupUpdateRate, value); }
        }               // milliseconds
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
            set { SetAndNotify(ref _isCollecting, value); }
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
        // Guard Properties
        private bool _canDataCollectionEnable;
        public bool CanDataCollectionEnable
        {
            get { return _canDataCollectionEnable; }
            set { SetAndNotify(ref _canDataCollectionEnable, value); }
        }	

        #endregion

        #region Constructor
        public ShellViewModel(IWindowManager windowManager, IDialogFactory dialogFactory)
        {
            // Property initialization
            // Stylet
            this.windowManager = windowManager;
            this.dialogFactory = dialogFactory;
            // Tags and Data
            IsCollecting = false;
            Data = new List<Data>();
            Tags = new BindableCollection<Tag>();
            Tags.CollectionChanged += Tags_CollectionChanged;
            // Plot
            WpfPlot = new WpfPlot();
            WpfPlot.Plot.YAxis.LockLimits();
            AutoScroll = true;
            //XLocked = true;
            //YLocked = true;
            PlotTimeSpan = 120;
            // OPC
            IsConnected = false;
            GroupUpdateRate = 500;
            Server = null;

            // Testing code
            PlotAllTags(PlotTimeSpan);
            WpfPlot.Refresh();
        }
        #endregion

        #region Actions
        // OPC
        public async Task DataCollectionEnable()
        {
            var definitions = new List<OpcDaItemDefinition>();
            foreach (Tag t in Tags)
                definitions.Add(new OpcDaItemDefinition() { ItemId = t.TagId, IsActive = true });

            IsCollecting = true;
            Group = Server.AddGroup("TagGroup");

            foreach (OpcDaItemDefinition definition in definitions)
            {
                OpcDaItemResult[] result = Group.AddItems(new List<OpcDaItemDefinition>() { definition });
                if (result[0].Error.Failed)
                {
                    MessageBox.Show(String.Format("Adding item failed:\n{0}\n\nError:\n{1}", definition.ItemId, result[0].Error.ToString()),
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public async Task DataCollectionDisable()
        {
            Server.RemoveGroup(Group);
            IsCollecting = false;
        }

        public async Task Disconnect()
        {
            if (Server != null)
            {
                Server.Disconnect();
                if ((Group != null) && (Server.Groups.Count > 0))
                    Server.RemoveGroup(Group);
            }

            Server = null;
            IsConnected = false;
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

        // Plot
        //public async Task LockXAxis()
        //{
        //    WpfPlot.Plot.XAxis.LockLimits(XLocked = !XLocked);
        //}

        //public async Task LockYAxis()
        //{
        //    WpfPlot.Plot.YAxis.LockLimits(YLocked = !YLocked);
        //}

        public async Task SetAutoScroll()
        {
            AutoScroll = !AutoScroll;
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

            IsConnected = (Server != null) && (Server.IsConnected) ? true : false;
        }

        public async Task ShowTagManagerDialog()
        {
            var dialogVm = this.dialogFactory.CreateTagManagerDialog();
            dialogVm.dialogFactory = this.dialogFactory;
            dialogVm.windowManager = this.windowManager;
            dialogVm.Tags = Tags;
            dialogVm.CustomColors = CustomColors;
            var result = this.windowManager.ShowDialog(dialogVm);
        }
        #endregion

        #region Other Methods
        // OPC
        public void UpdateServerStatus()
        {
            if ((Server != null) && (Server.IsConnected == true))
                ServerStatus = "Connected to " + Server.ServerDescription;
            else
                ServerStatus = "Disconnected";
        }

        public void UpdateCanDataCollectionEnable()
        {
            if ((Server != null) && !IsCollecting && Server.IsConnected && (Tags.Count > 0))
                CanDataCollectionEnable = true;
            else
                CanDataCollectionEnable = false;
        }

        private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCanDataCollectionEnable();
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
        TagEditorDialogViewModel CreateTagEditorDialog();
    }
}
