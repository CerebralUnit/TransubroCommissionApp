using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class QuickbooksVendorAttribute : Attribute
    {
        private string type; 

        public QuickbooksVendorAttribute( string vendorType ) {

            type = vendorType;
          
        }

        public string GetVendorType()
        {
            return type;
        } 
    }
}
