using Stylet;
using System;

namespace Collect.Views.Dialogs
{
    public class DatalogDialogViewModel : Screen
    {
        private int _updateRate;
        public int UpdateRate
        {
            get { return _updateRate; }
            set { SetAndNotify(ref _updateRate, value); }
        }


        public DatalogDialogViewModel()
        {
            this.DisplayName = "I'm Dialog 1";
        }

        public void Close()
        {
            this.RequestClose(null);
        }

        public void Save()
        {
            this.RequestClose(true);
        }
    }
}
