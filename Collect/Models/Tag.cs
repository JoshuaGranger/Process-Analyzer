using Stylet;
using System.Drawing;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Models
{
    class Tag : PropertyChangedBase
    {
        #region Properties
        // OPC Properties
        private OpcDaItemDefinition _definition;
        public OpcDaItemDefinition Definition
        {
            get { return _definition; }
            set { this.SetAndNotify(ref this._definition, value); }
        }
        // Plot Properties
        private Color _traceColor;
        public Color TraceColor
        {
            get { return _traceColor; }
            set { this.SetAndNotify(ref this._traceColor, value); }
        }
        private bool _isHidden;
        public bool IsHidden
        {
            get { return _isHidden; }
            set { this.SetAndNotify(ref this._isHidden, value); }
        }
        private double _plotMin;
        public double PlotMin
        {
            get { return _plotMin; }
            set { this.SetAndNotify(ref this._plotMin, value); }
        }
        private double _plotMax;
        public double PlotMax
        {
            get { return _plotMax; }
            set { this.SetAndNotify(ref this._plotMax, value); }
        }
        #endregion
    }
}
