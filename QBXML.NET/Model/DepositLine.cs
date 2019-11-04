using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class DepositLine
    {
        public string TxnType { get; set; }
        public string TxnLineId { get; set; }
        public string Memo { get; set; }
        public string CheckNumber { get; set; }
        public decimal Amount { get; set; }

        public QuickbooksAccount Account { get; set; } 
        public PaymentMethod Method { get; set; }
        public QuickbooksEntity Entity { get; set; }
    }
}
