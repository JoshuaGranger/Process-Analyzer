using ScottPlot.Plottable;
using Stylet;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Models
{
    public class Tag : PropertyChangedBase
    {
        #region Properties
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
        private TagBase _tagBase;
        public TagBase TagBase
        {
            get { return _tagBase; }
            set { SetAndNotify(ref _tagBase, value); }
        }
        #endregion

        #region Constructor
        public Tag(string tagName, Color color, double scaleYMin, double scaleYMax)
        {
            // Tag
            Definition = new OpcDaItemDefinition();
            Definition.ItemId = tagName;
            Definition.IsActive = true;
            Data = new List<double[]>();
            // TagBase
            TagBase = new TagBase();
            TagBase.ShowTrace = true;
            TagBase.TraceColor = color;
            TagBase.TagName = tagName;
            TagBase.TagShortDesc = tagName;
            TagBase.ScaleOverride = false;
            TagBase.PlotYMin = scaleYMin;
            TagBase.PlotYMax = scaleYMax;
        }
        #endregion
    }
}
