using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{ 
    public class PurchaseOrderAddResponse : QuickbooksResponseBase
    {
        public string TxnId { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public long EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        public string Vendor { get; set; } 
        public List<PurchaseOrderLine> Lines { get; set; }
    }
}
