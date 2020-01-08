using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    public class Client
    {
        public Client()
        {
            SalespersonSplits = new List<SalesCommission>();
        }

        public string Name { get; set; }
        public DateTime? ThresholdDate { get; set; }
        public decimal ClientPercentageOld { get; set; }
        public decimal ClientPercentageNew { get; set; }
        public decimal TransubroPercentageOld
        {
            get { return 1 - ClientPercentageOld; }
        }
        public decimal TransubroPercentageNew
        {
            get { return 1 - ClientPercentageNew; }
        }
        public List<SalesCommission> SalespersonSplits { get; set; }
    }

    
}
