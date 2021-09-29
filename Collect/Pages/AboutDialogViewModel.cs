using Stylet;
using System;

namespace Collect.Pages
{
    public class AboutDialogViewModel : Screen
    {
        public AboutDialogViewModel()
        {
            // test
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
