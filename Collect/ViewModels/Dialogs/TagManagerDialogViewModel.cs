using Collect.Models;
using Stylet;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
        public void MoveUp()
        {
            var index = Tags.IndexOf(SelectedTag);
            var tag = SelectedTag;
            Tags.RemoveAt(index);
            Tags.Insert(index - 1, tag);
            SelectedTag = Tags[index - 1];
        }

        public void MoveDown()
        {
            var index = Tags.IndexOf(SelectedTag);
            var tag = SelectedTag;
            Tags.RemoveAt(index);
            Tags.Insert(index + 1, tag);
            SelectedTag = Tags[index + 1];
        }

        public void Add()
        {

        }

        public void Delete()
        {

        }

        public void Import()
        {

        }

        public void Export()
        {

        }
        #endregion

        #region Other Methods
        private int NextAvailableID()
        {
            for (int i = 0; ; i++)
                if (Tags.Where(t => t.UniqueID == i).FirstOrDefault() == null)
                    return i;
        }
        #endregion

        public void Save()
        {
            this.RequestClose(true);
        }

        public void Close()
        {
            this.RequestClose(null);
        }
    }
}
