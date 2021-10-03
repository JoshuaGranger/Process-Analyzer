using Stylet;
using System.Collections.Generic;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Models
{
    public class Tag : PropertyChangedBase, ITagBase
    {
        #region Properties
        // TagBase
        private string _tagName;
        public string TagName
        {
            get { return _tagName; }
            set { SetAndNotify(ref _tagName, value); }
        }
        private string _tagShortDesc;
        public string TagShortDesc
        {
            get { return _tagShortDesc; }
            set { SetAndNotify(ref _tagShortDesc, value); }
        }
        private System.Drawing.Color _traceColor;
        public System.Drawing.Color TraceColor
        {
            get { return _traceColor; }
            set { SetAndNotify(ref _traceColor, value); }
        }	
        // Other
        private System.Windows.Media.Color _brushColor;
        public System.Windows.Media.Color BrushColor
        {
            get { return _brushColor; }
            set { SetAndNotify(ref _brushColor, value); }
        }
        private OpcDaItemDefinition _definition;
        public OpcDaItemDefinition Definition
        {
            get { return _definition; }
            set { this.SetAndNotify(ref this._definition, value); }
        }
        private List<double[]> _data;
        public List<double[]> Data
        {
            get { return _data; }
            set { SetAndNotify(ref _data, value); }
        }
        private double _plotYMax;
        public double PlotYMax
        {
            get { return _plotYMax; }
            set { SetAndNotify(ref _plotYMax, value); }
        }
        private double _plotYMin;
        public double PlotYMin
        {
            get { return _plotYMin; }
            set { SetAndNotify(ref _plotYMin, value); }
        }
        private bool _scaleOverride;
        public bool ScaleOverride
        {
            get { return _scaleOverride; }
            set { SetAndNotify(ref _scaleOverride, value); }
        }
        private bool _showTrace;
        public bool ShowTrace
        {
            get { return _showTrace; }
            set { SetAndNotify(ref _showTrace, value); }
        }
        #endregion

        #region Constructor
        // Constructor for application testing
        public Tag(string tagName, System.Drawing.Color color, double scaleYMin, double scaleYMax)
        {
            BrushColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            Definition = new OpcDaItemDefinition();
            Definition.ItemId = tagName;
            Definition.IsActive = true;
            Data = new List<double[]>();
            PlotYMax = scaleYMax;
            PlotYMin = scaleYMin;
            ScaleOverride = false;
            ShowTrace = true;
            TagName = tagName;
            TagShortDesc = tagName;
            TraceColor = color;
        }

        // GUI Constructor
        public Tag(OpcDaItemDefinition definition, string tagShortDesc, System.Drawing.Color color)
        {
            // Base
            TagName = definition.ItemId;
            TagShortDesc = TagName;
            TraceColor = System.Drawing.Color.Black;
            // Others
            Definition = definition;
            Definition.IsActive = true;
            Data = new List<double[]>();
            PlotYMax = 0;
            PlotYMin = 0;
            ScaleOverride = false;
            ShowTrace = true;
            BrushColor = System.Windows.Media.Color.FromArgb(TraceColor.A, TraceColor.R, TraceColor.G, TraceColor.B);
        }
        #endregion
    }
}
