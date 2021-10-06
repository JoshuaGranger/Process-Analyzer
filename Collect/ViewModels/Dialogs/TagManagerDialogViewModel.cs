using Collect.Models;
using Stylet;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Collect.Pages
{
    public class TagManagerDialogViewModel : Screen
    {
        #region Properties
        // Tags
        private BindableCollection<Tag> _tags;
        public BindableCollection<Tag> Tags
        {
            get { return _tags; }
            set { SetAndNotify(ref _tags, value); }
        }	
        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                SetAndNotify(ref _selectedTag, value);
                CanEdit = SelectedTag != null;
                CanDelete = SelectedTag != null;
                CanDuplicate = SelectedTag != null;
                CanMoveUp = (Tags.IndexOf(SelectedTag) > 0);
                CanMoveDown = (Tags.IndexOf(SelectedTag) < (Tags.Count - 1));
            }
        }
        public int[] CustomColors;
        // Guard Properties
        private bool _canEdit;
        public bool CanEdit
        {
            get { return _canEdit; }
            set { SetAndNotify(ref _canEdit, value); }
        }
        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetAndNotify(ref _canDelete, value); }
        }	
        private bool _canDuplicate;
        public bool CanDuplicate
        {
            get { return _canDuplicate; }
            set { SetAndNotify(ref _canDuplicate, value); }
        }	
        private bool _canMoveUp;
        public bool CanMoveUp
        {
            get { return _canMoveUp; }
            set { SetAndNotify(ref _canMoveUp, value); }
        }
        private bool _canMoveDown;
        public bool CanMoveDown
        {
            get { return _canMoveDown; }
            set { SetAndNotify(ref _canMoveDown, value); }
        }
        #endregion

        #region Actions
        public async Task MoveUp()
        {
            var index = Tags.IndexOf(SelectedTag);
            var tag = SelectedTag;
            Tags.RemoveAt(index);
            Tags.Insert(index - 1, tag);
            SelectedTag = Tags[index - 1];
        }

        public async Task MoveDown()
        {
            var index = Tags.IndexOf(SelectedTag);
            var tag = SelectedTag;
            Tags.RemoveAt(index);
            Tags.Insert(index + 1, tag);
            SelectedTag = Tags[index + 1];
        }

        public async Task Add()
        {
            //Tags.Add(new Tag(new OpcDaItemDefinition(), "", System.Drawing.Color.Black));
            //SelectedTag = Tags.Last();
        }

        public async Task Delete()
        {
            Tags.Remove(SelectedTag);
        }

        public async Task Import()
        {

        }

        public async Task Export()
        {

        }

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
            this.RequestClose(true);
        }

        public void Close()
        {
            this.RequestClose(true);
        }
    }
}
