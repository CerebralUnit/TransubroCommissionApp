using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class PurchaseOrder 
    {
        public string VendorName { get; set; }
        public List<PurchaseOrderLine> Lines { get; set; }
    }
}
