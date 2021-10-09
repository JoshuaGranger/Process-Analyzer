using Collect.Models;
using Stylet;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Views
{
    public class TagDialogViewModel : Screen, ITagBase
    {
        #region Properties
        // Tag Properties
        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set { SetAndNotify(ref _selectedTag, value); }
        }
        private string tagId;
        public string TagId
        {
            get { return tagId; }
            set { SetAndNotify(ref tagId, value); }
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
        // View Properties
        public int[] CustomColors;
        #endregion

        #region Actions
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
        #endregion

        public void Save()
        {
            SelectedTag.TagId = TagId;
            SelectedTag.TagDesc = TagDesc;
            SelectedTag.TraceColor = TraceColor;
            this.RequestClose(true);
        }

        public void Close()
        {
            this.RequestClose(true);
        }
    }
}
