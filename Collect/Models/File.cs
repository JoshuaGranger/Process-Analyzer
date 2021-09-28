using Stylet;
using System;

namespace Collect.Models
{
    class File : PropertyChangedBase
    {
        #region Properties
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { this.SetAndNotify(ref this._filePath, value); }
        }
        private DateTime _fileDateTime;
        public DateTime FileDateTime
        {
            get { return _fileDateTime; }
            set { this.SetAndNotify(ref this._fileDateTime, value); }
        }
        private IObservableCollection<Datum> _data;
        public IObservableCollection<Datum> Data
        {
            get { return _data; }
            set { this.SetAndNotify(ref this._data, value); }
        }
        private string _fileName;
        #endregion

        // Constructor
        public File(string filepath)
        {
            FilePath = filepath;
            FileDateTime = DateTime.Now;
            _fileName = "collected_data_" + FileDateTime.ToString("yyyyMMddTHHmmss") +".txt";
            Data = new BindableCollection<Datum>();
        }
    }
}
