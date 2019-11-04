using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class SalesOrPurchase
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public QuickbooksAccount Account { get; set; }
    }
}
