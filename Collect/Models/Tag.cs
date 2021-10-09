using Stylet;
using System.Collections.Generic;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Models
{
    public class Tag : PropertyChangedBase, ITagBase
    {
        #region Properties
        // TagBase
        private string tagId;
        public string TagId
        {
            get { return tagId; }
            set
            {
                SetAndNotify(ref tagId, value); 
            }
        }
        private string _tagDesc;
        public string TagDesc
        {
            get { return _tagDesc; }
            set { SetAndNotify(ref _tagDesc, value); }
        }
        private System.Drawing.Color _traceColor;
        public System.Drawing.Color TraceColor
        {
            get { return _traceColor; }
            set { SetAndNotify(ref _traceColor, value); }
        }	
        // Other
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
        // GUI Constructor
        public Tag(string tagId, string tagDesc, System.Drawing.Color color)
        {
            // Others
            TagId = tagId;
            TagDesc = tagDesc;
            TraceColor = color;
            PlotYMax = 0;
            PlotYMin = 0;
            ScaleOverride = false;
            ShowTrace = true;
        }
        #endregion
    }
}
