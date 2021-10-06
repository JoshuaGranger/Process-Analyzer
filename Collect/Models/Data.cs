using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collect.Models
{
    public class Data
    {
        public string TagId { get; set; }
        public List<double[]> Points { get; set; }

        public Data(string tagId)
        {
            TagId = tagId;
            Points = new List<double[]>();
        }
    }
}
