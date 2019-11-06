using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Model
{
    public class DepositQueryResponse : QuickbooksResponseBase
    {
        public List<QuickbooksDeposit> Deposits { get; set; }
    }
}
