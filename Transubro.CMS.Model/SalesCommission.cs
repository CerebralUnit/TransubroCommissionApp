using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    public class SalesCommission
    {
        public string SalesPersonName { get; set; }
        public string AmountType { get; set; }
        public decimal? Amount { get; set; }
    }
}
