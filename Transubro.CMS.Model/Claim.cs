using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    public class Claim
    {
        public string FileNumber { get; set; }
        public string Description { get; set; }
        public decimal CheckAmount { get; set; } 
    }
}
