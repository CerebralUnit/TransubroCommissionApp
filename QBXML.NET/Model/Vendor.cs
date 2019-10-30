using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class Vendor
    {
        public string ListId { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
    }
}
