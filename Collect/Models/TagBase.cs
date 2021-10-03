using Stylet;

namespace Collect.Models
{
    interface ITagBase
    {
        string TagName { get; set; }
        string TagShortDesc { get; set; }
        System.Drawing.Color TraceColor { get; set; }
    }
}
