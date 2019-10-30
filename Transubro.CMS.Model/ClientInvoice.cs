using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    public class ClientInvoice
    {
        public string Client { get; set; }
        List<Claim> Claims { get; set; }
    }
}
