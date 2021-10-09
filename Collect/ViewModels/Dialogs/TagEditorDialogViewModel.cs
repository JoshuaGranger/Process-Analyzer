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
    public class TagEditorDialogViewModel : Screen, ITagBase
    {
        #region Properties
        // Tag Properties
        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                SetAndNotify(ref _selectedTag, value);
                if (SelectedTag != null)
                {
                    TagId = SelectedTag.TagId;
                    TagDesc = SelectedTag.TagDesc;
                    TraceColor = SelectedTag.TraceColor;
                }
            }
        }
        public string[] TakenTags;
        private string tagId;
        public string TagId
        {
            get { return tagId; }
            set
            {
                SetAndNotify(ref tagId, value);
                CanSave = (!TakenTags.Contains(TagId)) && (TagId != "");
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
        // Guard Properties
        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set { SetAndNotify(ref _canSave, value); }
        }	
        // View Properties
        public int[] CustomColors;
        #endregion

        #region Actions
        public async Task ColorChange()
        {
            // TODO: make the color changing mechanism more WPF/MVVM-like
            var colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.SolidColorOnly = true;
            colorDialog.FullOpen = true;

            if (CustomColors != null)
                colorDialog.CustomColors = CustomColors;

            var dialogResult = colorDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                TraceColor = colorDialog.Color;
            }
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
            this.RequestClose(null);
        }
    }
}
