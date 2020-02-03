using QBXML.NET.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    [QuickbooksPurchaseOrder]
    public class Invoice
    {
        public string ClientName { get; set; }
        public List<InvoiceLine> Lines { get; set; }
       
    }
}
