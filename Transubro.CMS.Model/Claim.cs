using QBXML.NET;
using QBXML.NET.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{
    [QuickbooksItem(ItemType.Service, "Subrogation Client")]
    public class Claim
    {
        [QuickbooksField("Name")]
        public string FileNumber { get; set; }

        [QuickbooksField("Description")]
        public string Description { get; set; }

        [QuickbooksField("Price")]
        public decimal CheckAmount { get; set; }

        public decimal ClientPercentDecimal { get; set; }

        public decimal ClientDue
        {
            get
            {
                return CheckAmount * ClientPercentDecimal;
            }
        }
    }
}
