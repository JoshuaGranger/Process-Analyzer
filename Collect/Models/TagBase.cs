using Stylet;

namespace Collect.Models
{
    interface ITagBase
    {
        string TagId { get; set; }
        string TagDesc { get; set; }
        System.Drawing.Color TraceColor { get; set; }
    }
}
