using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    public class ClientInvoice
    {
        public ClientInvoice()
        {
            Claims = new List<Claim>();
        }
        public string Client { get; set; }
        public List<Claim> Claims { get; set; }
        public List<QBXML.NET.Model.DepositLine> Checks { get; set; }
    }
}
