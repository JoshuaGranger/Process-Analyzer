using Stylet;

namespace Collect.Models
{
    public class TagBase : PropertyChangedBase
    {
        private bool _showTrace;
        public bool ShowTrace
        {
            get { return _showTrace; }
            set { SetAndNotify(ref _showTrace, value); }
        }
        private System.Drawing.Color _traceColor;
        public System.Drawing.Color TraceColor
        {
            get { return _traceColor; }
            set { SetAndNotify(ref _traceColor, value); }
        }
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
        private bool _scaleOverride;
        public bool ScaleOverride
        {
            get { return _scaleOverride; }
            set { SetAndNotify(ref _scaleOverride, value); }
        }
        private double _plotYMin;
        public double PlotYMin
        {
            get { return _plotYMin; }
            set { SetAndNotify(ref _plotYMin, value); }
        }
        private double _plotYMax;
        public double PlotYMax
        {
            get { return _plotYMax; }
            set { SetAndNotify(ref _plotYMax, value); }
        }
    }
}
