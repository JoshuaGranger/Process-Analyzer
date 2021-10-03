using Stylet;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Collect.Pages
{
    public class TagManagerDialogViewModel : Screen
    {
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
