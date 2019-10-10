using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSubroCommissions
{
    public class Client
    {
        public string Name { get; set; }
        public DateTime ThresholdDate { get; set; }
        public decimal TransubroPercentageOld { get; set; }
        public decimal TransubroPercentageNew { get; set; }
        public decimal ClientPercentageOld
        { 
            get { return 1 - TransubroPercentageOld; }
        }
        public decimal ClientPercentageNew
        {
            get { return 1 - TransubroPercentageNew; }
        }
    }
}
