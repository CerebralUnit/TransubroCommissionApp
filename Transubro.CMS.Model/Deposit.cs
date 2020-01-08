using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    public class Deposit
    {
        public Deposit() { }

        public Deposit(List<Check> checks)
        {
            this.Checks = checks;
        }

        public List<Check> Checks { get; set; }
    }
}
