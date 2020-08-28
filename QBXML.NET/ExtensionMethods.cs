using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBXML.NET
{
    public static class ExtensionMethods
    {
        public static string ToXmlString(this object obj)
        {
            return obj.ToString().ToXmlString();
                        
        }

        public static string ToXmlString(this string str)
        {
            if (str == null)
                return String.Empty;

            return str
                    .Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("&#34;", "&quot;")
                    .Replace("'", "&apos;");
        }
    }
}
