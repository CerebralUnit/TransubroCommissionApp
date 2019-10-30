using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class QuickbooksItemAttribute : Attribute
    {
        private ItemType type;
        private string account;

        public QuickbooksItemAttribute(ItemType itemType, string accountName) {

            type = itemType;
            account = accountName;
        }

        public ItemType GetItemType()
        {
            return type;
        }

        public string GetAccount()
        {
            return account;
        }
    }
}
