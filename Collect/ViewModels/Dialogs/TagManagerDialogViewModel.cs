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
    public class TagManagerDialogViewModel : Screen
    {
        #region Properties
        // Stylet
        private IDialogFactory dialogFactory;
        private IWindowManager windowManager;
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
                IsSelected = (SelectedTag != null);
                CanExport = Tags.Count > 0;
            }
        }
        // View Properties
        public int[] CustomColors;
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetAndNotify(ref _isSelected, value); }
        }	
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
        private bool _canImport;
        public bool CanImport
        {
            get { return false; }
            set { SetAndNotify(ref _canImport, value); }
        }
        private bool _canExport;
        public bool CanExport
        {
            get { return false; }
            set { SetAndNotify(ref _canExport, value); }
        }
        #endregion

        public TagManagerDialogViewModel(IWindowManager windowManager, IDialogFactory dialogFactory)
        {
            // Property initialization
            // Stylet
            this.windowManager = windowManager;
            this.dialogFactory = dialogFactory;
        }

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
            OpcDaItemDefinition def = new OpcDaItemDefinition();
            def.IsActive = false;
            var tag = new Tag(def, "", System.Drawing.Color.RoyalBlue);
            SelectedTag = tag;

            ShowTagDialog();
        }

        public async Task Delete()
        {
            Tags.Remove(SelectedTag);
            SelectedTag = (Tags.Count > 0) ? Tags[Tags.Count - 1] : null;
        }

        public async Task Import()
        {

        }

        public async Task Export()
        {

        }
        #endregion

        public async Task ShowTagDialog()
        {
            var dialogVm = this.dialogFactory.CreateTagDialog();
            dialogVm.SelectedTag = SelectedTag;
            dialogVm.CustomColors = CustomColors;
            var result = this.windowManager.ShowDialog(dialogVm);
        }

        public void Save()
        {
            this.RequestClose(true);
        }

        public void Close()
        {
            this.RequestClose(true);
        }

        public interface IDialogFactory
        {
            TagDialogViewModel CreateTagDialog();
        }
    }
}
