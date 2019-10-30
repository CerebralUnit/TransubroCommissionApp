using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Attributes
{
    public class QuickbooksField : Attribute
    {
        private string name;

        public QuickbooksField(string Name) {
            name = Name;
        }

        public string GetName()
        {
            return name;
        }
    }
}
