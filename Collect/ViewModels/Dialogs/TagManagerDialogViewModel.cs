using Collect.Models;
using Stylet;
using System;
using System.Collections.Generic;
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
        public Collect.Views.IDialogFactory dialogFactory;
        public IWindowManager windowManager;
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
        private Tag _lastSelected;
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
            _lastSelected = SelectedTag;
            var takenTagList = Tags.Select(x => x.TagId).ToArray();
            var tag = new Tag("", "", System.Drawing.Color.RoyalBlue);
            SelectedTag = tag;

            ShowTagDialog(takenTagList);
        }

        public async Task Edit()
        {
            _lastSelected = SelectedTag;
            List<string> tagList = Tags.Select(x => x.TagId).ToList();
            tagList.Remove(SelectedTag.TagId);

            ShowTagDialog(tagList.ToArray());
        }

        public async Task Duplicate()
        {
            _lastSelected = SelectedTag;
            var takenTagList = Tags.Select(x => x.TagId).ToArray();
            var tag = new Tag(SelectedTag.TagId, SelectedTag.TagDesc, SelectedTag.TraceColor);
            SelectedTag = tag;

            ShowTagDialog(takenTagList);
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

        public async Task ShowTagDialog(string[] takenTagList)
        {
            var dialogVm = this.dialogFactory.CreateTagEditorDialog();
            dialogVm.TakenTags = takenTagList;
            dialogVm.SelectedTag = SelectedTag;
            dialogVm.CustomColors = CustomColors;
            
            var result = this.windowManager.ShowDialog(dialogVm);
            if (result == true)
            {
                if (!Tags.Contains(SelectedTag))
                    Tags.Add(SelectedTag);

                // Trigger propertychanged to update derived properties
                SelectedTag = SelectedTag;
            }
            else
            {
                if (_lastSelected != null)
                    SelectedTag = _lastSelected;
            }

        }

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
