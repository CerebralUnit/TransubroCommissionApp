using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public abstract class QuickbooksEntityBase
    {
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public long EditSequence { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
