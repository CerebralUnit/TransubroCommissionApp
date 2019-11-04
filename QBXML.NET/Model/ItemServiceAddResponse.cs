using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class ItemServiceAddResponse : QuickbooksResponseBase
    {
        public string ListId { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public long EditSequence { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public int Sublevel { get; set; }
        public SalesOrPurchase Details { get; set; } 
    } 
}
