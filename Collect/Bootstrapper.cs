using System;
using Stylet;
using StyletIoC;
using Collect.Views;
using Collect.Views.Dialogs;

namespace Collect
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);

            builder.Bind<IDialogFactory>().ToAbstractFactory();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
