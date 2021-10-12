using Collect.Models;
using Collect.Services;
using Microsoft.Win32;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

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
            set
            {
                SetAndNotify(ref _tags, value);
                if (Tags != null)
                    Tags.CollectionChanged += Tags_CollectionChanged;
            }
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
        private bool _canExport;
        public bool CanExport
        {
            get { return _canExport; }
            set { SetAndNotify(ref _canExport, value); }
        }
        #endregion

        #region Actions
        public async Task Add()
        {
            _lastSelected = SelectedTag;
            var takenTagList = Tags.Select(x => x.TagId).ToArray();
            var tag = new Tag("", "", System.Drawing.Color.RoyalBlue);
            SelectedTag = tag;

            ShowTagDialog(takenTagList);
        }

        public async Task Delete()
        {
            Tags.Remove(SelectedTag);
            SelectedTag = (Tags.Count > 0) ? Tags[Tags.Count - 1] : null;
        }

        public async Task Duplicate()
        {
            _lastSelected = SelectedTag;
            var takenTagList = Tags.Select(x => x.TagId).ToArray();
            var tag = new Tag(SelectedTag.TagId, SelectedTag.TagDesc, SelectedTag.TraceColor);
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

        public async Task Export()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string uriPath = Uri.UnescapeDataString(uri.Path);
            string startPath = Path.GetDirectoryName(uriPath);
            //string startPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string defaultFileName = DateTime.Now.ToString("yyyyMMddTHHmmss") + "_Collect_TagExport.json";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = startPath;
            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "JSON | *.json";
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                TagService.Export(Tags, saveFileDialog.FileName);
            }
        }

        public async Task Import()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string uriPath = Uri.UnescapeDataString(uri.Path);
            string startPath = Path.GetDirectoryName(uriPath);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = startPath;
            openFileDialog.DefaultExt = "json";
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "JSON | *.json";
            openFileDialog.CheckFileExists = true;
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    TagService.Import(Tags, openFileDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("Exception encountered during import:\n\n{0}", e.Message), "Import Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }

        public async Task MoveDown()
        {
            var index = Tags.IndexOf(SelectedTag);
            var tag = SelectedTag;
            Tags.RemoveAt(index);
            Tags.Insert(index + 1, tag);
            SelectedTag = Tags[index + 1];
        }

        public async Task MoveUp()
        {
            var index = Tags.IndexOf(SelectedTag);
            var tag = SelectedTag;
            Tags.RemoveAt(index);
            Tags.Insert(index - 1, tag);
            SelectedTag = Tags[index - 1];
        }

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
        #endregion

        #region Other Methods
        private void Tags_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExport = Tags.Count > 0;
        }
        #endregion

        public void Close()
        {
            this.RequestClose(true);
        }
    }
}
