using QBXML.NET;
using QBXML.NET.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transubro.CMS.Model
{ 
    [QuickbooksDeposit]
    public class Check
    {
        private string memo = "LOU";

        [QuickbooksField("Memo")]
        public string Memo
        {
            get
            {
                if (LossOfUseAmount > 0 && PropertyDamageAmount > 0)
                    memo = " COMBO";
                else if (PropertyDamageAmount > 0)
                    memo = " PD";
                else
                    memo =  " LOU";

                return FileNumber + memo;
            }
        }

        [QuickbooksField("DepositLineEntityFullName")]
        public string ReceivedFrom { get; set; }

        [QuickbooksField("AccountRef")]
        public string FromAccount { get; set; }

        [QuickbooksField("DepositToAccount")] 
        public string To { get; set; }

        [QuickbooksField("CheckNumber")]
        public string CheckNumber { get; set; }
         
        [QuickbooksField("DepositLineMemo")]
        public string FileNumber { get; set; } 

        public string LossOfUseDescription { get; set; }
         
        public decimal? LossOfUseAmount { get; set; }

        public string PropertyDamageDescription { get; set; }

        public decimal? PropertyDamageAmount { get; set; }

        public string OtherDescription { get; set; }

        public decimal? OtherAmount { get; set; }

        [QuickbooksField("Amount")]
        public decimal CheckAmount {
            get
            {
                decimal value = 0;

                value += LossOfUseAmount.HasValue ? LossOfUseAmount.Value : 0;
                value += PropertyDamageAmount.HasValue ? PropertyDamageAmount.Value : 0;
                value += OtherAmount.HasValue ? OtherAmount .Value : 0;

                return value;
            }
        }
    }
}
