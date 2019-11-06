using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class Employee : QuickbooksEntityBase
    { 
		public string FirstName { get; set; }
		public string LastName { get; set; } 
		public string PrintAs { get; set; }
		public string EmployeeType { get; set; }
        public string PayPeriod { get; set; }
		public DateTime HiredDate { get; set; }

        public List<PayrollItem> Earnings { get; set;  }
    }

    public class PayrollItem
    {
        public string Type { get; set; }
        public string ListId { get; set; }
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public string AmountType { get; set; }
    }
}
