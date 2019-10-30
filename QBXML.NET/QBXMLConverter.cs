using QBXML.NET.Attributes;
using QBXML.NET.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QBXML.NET
{
    public class QBXMLConverter
    {
        private const string ItemServiceAddTemplate = @"XMLTemplates\ItemServiceAddRequest.xml";
        private const string ItemQueryDateRangeTemplate = @"XMLTemplates\ItemQueryByDateRange.xml";
        private const string ItemQueryDateRangePrefixTemplate = @"XMLTemplates\ItemQueryByDateRangeAndPrefix.xml";
        private const string EnvelopeTemplate = @"XMLTemplates\QBXMLEnvelope.xml";
        private const string VendorQueryTypeTemplate = @"XMLTemplates\VendorQueryByType.xml";
        private const string DepositTemplate = @"XMLTemplates\Deposit.xml";
        private const string DepositQueryTemplate = @"XMLTemplates\DepositQuery.xml";
        private const string PayrollItemQueryTemplate = @"XMLTemplates\PayrollItemQuery.xml";
        private const string EmployeeQueryTemplate = @"XMLTemplates\EmployeeQueryByName.xml"; 
        private const string CustomerQueryTemplate = @"XMLTemplates\CustomerQuery.xml"; 


        public string ConvertItem(object item )
        {
            string xml = "";

            Type t = item.GetType();

            QuickbooksItemAttribute qbItemAttribute = (QuickbooksItemAttribute) Attribute.GetCustomAttribute(t , typeof(QuickbooksItemAttribute));

            ItemType itemType = qbItemAttribute.GetItemType();

            string accountName = qbItemAttribute.GetAccount();

            var fields = GetQuickbooksFieldValues(item, t);

            if (FieldsAreValidForServiceItemAdd(fields))
            {
                var template = GetTemplateText(ItemServiceAddTemplate);

                if(template != null)
                {
                    xml = template
                            .Replace("{{Name}}", fields["Name"].ToString())
                            .Replace("{{Description}}", fields["Description"].ToString())
                            .Replace("{{Price}}", fields["Price"].ToString())
                            .Replace("{{AccountName}}", accountName);
                } 
            }

            return xml;
        }

        public string ConvertDeposit<T>(T item)
        {
            string xml = "";

            Type t = typeof(T);

            QuickbooksDepositAttribute qbDepositAttribute = (QuickbooksDepositAttribute)Attribute.GetCustomAttribute(t, typeof(QuickbooksDepositAttribute));
             
            var fields = GetQuickbooksFieldValues(item, t);

            if (FieldsAreValidForDepositAdd(fields))
            {
                var template = GetTemplateText(DepositTemplate);

                if (template != null)
                {
                    xml = template
                        .Replace("{{Date}}", DateTime.Now.ToString("yyyy-MM-dd"))
                        .Replace("{{DepositToAccount}}", "America First")
                        .Replace("{{Memo}}", fields["Memo"].ToString())
                        .Replace("{{DepositLineEntityFullName}}", fields["DepositLineEntityFullName"].ToString())
                        .Replace("{{DepositLineMemo}}", fields["DepositLineMemo"].ToString())
                        .Replace("{{CheckNumber}}", fields["CheckNumber"].ToString())
                        .Replace("{{Amount}}", ((decimal)fields["Amount"]).ToString("F2") );
                }
            }

            return xml;
        }


        public string ConvertDeposit()
        {
            return GetTemplateText(DepositTemplate);
        }

        public string ConvertDepositQuery(DateTime startDate, DateTime endDate)
        {
            string template = GetTemplateText(DepositQueryTemplate);

            string xml = "";

            if(template != null)
            {
                xml = template
                        .Replace("{{StartDate}}", startDate.ToString("yyyy-MM-dd")) 
                        .Replace("{{EndDate}}", endDate.ToString("yyyy-MM-dd"));
            }

            return xml;
        }

        public string GetPayrollItemQuery()
        {
            var template = GetTemplateText(PayrollItemQueryTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template;
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
        }
        public string GetCustomersQuery()
        {
            var template = GetTemplateText(CustomerQueryTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template;
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
        }

        public List<Customer> DeserializeCustomerQueryResponse(string xml)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList customerResponseNodes = xmlDoc.GetElementsByTagName("CustomerRet");

            var response = new List<Customer>();

            foreach (XmlNode node in customerResponseNodes)
            {
                var customerTypeNode = node["CustomerTypeRef"];
                 
                response.Add(new Customer()
                {
                    FullName = node["FullName"].InnerText,
                    CustomerType = customerTypeNode["FullName"].InnerText 
                });
            }

            return response;
        }

        public string ConvertEmployeeQuery(string name)
        {
            var template = GetTemplateText(EmployeeQueryTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template
                        .Replace("{{Name}}", name);
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
        }
        public string ConvertVendorQuery(string vendorType)
        {
            var template = GetTemplateText(VendorQueryTypeTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template
                        .Replace("{{Type}}", vendorType);
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
        }
        public List<Vendor> DeserializeVendorQueryResponse(string xml)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList vendorResponseNodes = xmlDoc.GetElementsByTagName("VendorRet");

            var response = new List<Vendor>();
             
            foreach (XmlNode node in vendorResponseNodes)
            {
                var vendorTypeNode = node["VendorTypeRef"];
                var customFields = new Dictionary<string, string>();

                foreach(XmlNode child in node.ChildNodes)
                {
                    if(child.Name == "DataExtRet") 
                        customFields.Add( child["DataExtName"].InnerText, child["DataExtValue"].InnerText);  
                }

                response.Add(new Vendor()
                {
                    Name = node["Name"].InnerText,
                    ListId = node["ListID"].InnerText,
                    TimeCreated = DateTime.Parse(node["TimeCreated"].InnerText),
                    TimeModified = DateTime.Parse(node["TimeModified"].InnerText),
                    Type = vendorTypeNode["FullName"].InnerText,
                    Balance = decimal.Parse(node["Balance"].InnerText),
                    CustomFields = customFields
                }); 
            }

            return response;
        }
        public string GetPurchaseOrderXML()
        {
            return GetTemplateText(@"XMLTemplates\TestPurchaseOrderAdd.xml");
        }

        public string ConvertItemQueryByDateRangePrefix(string prefix, DateTime startDate, DateTime endDate)
        {
            var template = GetTemplateText(ItemQueryDateRangePrefixTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template
                        .Replace("{{StartDate}}", startDate.ToString("yyyy-MM-dd"))
                        .Replace("{{Prefix}}", startDate.ToString(prefix))
                        .Replace("{{EndDate}}", endDate.ToString("yyyy-MM-dd"));
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
        }

        public List<T> DeserializeItemQueryResponse<T>(string xml) where T : new()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList itemResponseNodes = xmlDoc.GetElementsByTagName("ItemServiceRet");
             
            var response = new List<T>();

            Type t = typeof(T);

            var propertyMap = GetQuickbooksFieldProperties(t);

            foreach (XmlNode node in itemResponseNodes)
            {
                var salesPurchaseNode = node["SalesOrPurchase"];

                var item = new T();
                 
                propertyMap["Name"].SetValue(item, node["Name"].InnerText);
                propertyMap["Description"].SetValue(item, salesPurchaseNode["Desc"].InnerText);
                propertyMap["Price"].SetValue(item, decimal.Parse(salesPurchaseNode["Price"].InnerText));

                response.Add(item);
            }
          
            return response;
        }

        public string ConvertItemQueryByDateRange(DateTime startDate, DateTime endDate)
        {
            var template = GetTemplateText(ItemQueryDateRangeTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template
                        .Replace("{{StartDate}}", startDate.ToString("yyyy-MM-dd"))
                        .Replace("{{EndDate}}", endDate.ToString("yyyy-MM-dd"));
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);  
        }

        

        public string ConvertItems(List<object> items)
        {
            string envelopeXml = GetTemplateText(EnvelopeTemplate);
            string addRequests = "";

            foreach (var item in items )
            {
                addRequests += ConvertItem(item);
            }

            return envelopeXml.Replace("{{Requests}}", addRequests); 
        }
         
        private string GetTemplateText(string templateFile)
        {
            string template = null;
            if (File.Exists(templateFile))
            {

                using (TextReader textReader = File.OpenText(templateFile))
                {
                    template = textReader.ReadToEnd(); 
                }

            }

            return template;
        }
        public bool FieldsAreValidForServiceItemAdd(Dictionary<string, object> fieldValues)
        {
            return fieldValues.ContainsKey("Name") && fieldValues.ContainsKey("Description") && fieldValues.ContainsKey("Price");
        }

        public bool FieldsAreValidForDepositAdd(Dictionary<string, object> fieldValues)
        {
            return fieldValues.ContainsKey("DepositLineEntityFullName") &&  
                   fieldValues.ContainsKey("Memo") &&  
                   fieldValues.ContainsKey("DepositLineMemo") && 
                   fieldValues.ContainsKey("CheckNumber") &&
                   fieldValues.ContainsKey("Amount");
        }

        private Dictionary<string, object> GetQuickbooksFieldValues (object instance, Type t )  
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();

            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    QuickbooksField authAttr = attr as QuickbooksField;

                    if (authAttr != null)
                    {
                        string propName = prop.Name;
                        object value = prop.GetValue(instance);
                        properties.Add(authAttr.GetName(), value);
                    }
                }
            }

            return properties;
        }

        private Dictionary<string, PropertyInfo> GetQuickbooksFieldProperties(Type t)
        {
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();

            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    QuickbooksField authAttr = attr as QuickbooksField;

                    if (authAttr != null)
                    {
                       
                        properties.Add(authAttr.GetName(), prop);
                    }
                }
            }

            return properties;
        }
    }
}
