using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class PayrollWageItem : QuickbooksEntityBase
    {
        public string WageType { get; set; }
        public QuickbooksAccount ExpenseAccount { get; set; }
    }
}
