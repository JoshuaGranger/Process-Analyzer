using Stylet;
using System;
using System.Threading.Tasks;

namespace Collect.Views
{
    public class RateDialogViewModel : Screen
    {
        private string _groupUpdateRate;
        public string GroupUpdateRate
        {
            get { return _groupUpdateRate; }
            set { SetAndNotify(ref _groupUpdateRate, value); UpdateCanSave(); }
        }
        private bool _canSave;
        public bool CanSave
        {
            get { return this._canSave; }
            set { this.SetAndNotify(ref this._canSave, value); }
        }

        public void UpdateCanSave()
        {
            bool convert = int.TryParse(GroupUpdateRate, out int result);
            CanSave = (convert && ((250 <= result) && (result <= 30000)));
        }

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
