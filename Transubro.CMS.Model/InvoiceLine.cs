using QBXML.NET.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    [QuickbooksPurchaseOrder]
    public class InvoiceLine
    {
        public string LineNumber { get; set; }
        public string FileNumber { get; set; }
        public string Description { get; set; }
        public string CheckAmount { get; set; }
        public string AmountDue { get; set; }
        public string CommissionRate { get; set; }
        public string SplitRate { get; set; }

        public decimal CompanyAmount { get; set; }

        public bool IsFlatCommission { get; set; }
    }
}
