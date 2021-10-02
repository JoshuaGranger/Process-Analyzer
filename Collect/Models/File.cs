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
        private string _fileName;
        private int _fileSize;
        #endregion

        // Constructor
        public File(string filepath)
        {
            FilePath = filepath;
            _fileDateTime = DateTime.Now;
            _fileName = "collected_data_" + _fileDateTime.ToString("yyyyMMddTHHmmss") +".txt";
            _fileSize = 0;
        }
    }
}
