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
        private const string ItemServiceAddTemplate           = @"XMLTemplates\ItemServiceAddRequest.xml";
        private const string ItemQueryDateRangeTemplate       = @"XMLTemplates\ItemQueryByDateRange.xml";
        private const string ItemQueryDateRangePrefixTemplate = @"XMLTemplates\ItemQueryByDateRangeAndPrefix.xml";
        private const string EnvelopeTemplate                 = @"XMLTemplates\QBXMLEnvelope.xml";
        private const string VendorQueryTypeTemplate          = @"XMLTemplates\VendorQueryByType.xml";
        private const string DepositTemplate                  = @"XMLTemplates\Deposit.xml";
        private const string DepositLineTemplate              = @"XMLTemplates\DepositLine.xml";
        private const string DepositQueryTemplate             = @"XMLTemplates\DepositQuery.xml";
        private const string PayrollItemQueryTemplate         = @"XMLTemplates\PayrollItemQuery.xml";
        private const string EmployeeQueryTemplate            = @"XMLTemplates\EmployeeQueryByName.xml"; 
        private const string CustomerQueryTemplate            = @"XMLTemplates\CustomerQuery.xml";
        private const string ItemPrefixFilterTemplate         = @"XMLTemplates\ItemQueryPrefixFilter.xml";
        private const string AccountQueryTemplate             = @"XMLTemplates\AccountQuery.xml";
        private const string PurchaseOrderAddTemplate         = @"XMLTemplates\PurchaseOrderAddRequest.xml";
        private const string PurchaseOrderLineAddTemplate     = @"XMLTemplates\PurchaseOrderLineAddRequest.xml";



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


        public string ConvertItem<T>(T item)
        {
            string xml = "";

            Type t = typeof(T);

            QuickbooksItemAttribute qbItemAttribute = (QuickbooksItemAttribute)Attribute.GetCustomAttribute(t, typeof(QuickbooksItemAttribute));

            ItemType itemType = qbItemAttribute.GetItemType();

            string accountName = qbItemAttribute.GetAccount();

            var fields = GetQuickbooksFieldValues(item, t);

            if (FieldsAreValidForServiceItemAdd(fields))
            {
                var template = GetTemplateText(ItemServiceAddTemplate);

                if (template != null)
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

        public string ConvertDeposit<T>(List<T> items)
        {
            string xml = "";

            Type t = typeof(T);

            QuickbooksDepositAttribute qbDepositAttribute = (QuickbooksDepositAttribute)Attribute.GetCustomAttribute(t, typeof(QuickbooksDepositAttribute));

            string linesXml = "";

            foreach(var item in items)
            {
                linesXml += ConvertDepositLine(item);
            }
            
            var template = GetTemplateText(DepositTemplate);

            if (template != null)
            {
                xml = template
                    .Replace("{{Date}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{DepositToAccount}}", "Operating Account - 3363")
                    .Replace("{{Memo}}", "Deposit")
                    .Replace("{{DepositLines}}", linesXml);
            } 

            return xml;
        }

        public string ConvertDepositLine<T>(T item)
        {
            string xml = "";

            Type t = typeof(T);

            QuickbooksDepositAttribute qbDepositAttribute = (QuickbooksDepositAttribute)Attribute.GetCustomAttribute(t, typeof(QuickbooksDepositAttribute));

            var fields = GetQuickbooksFieldValues(item, t);

            if (FieldsAreValidForDepositAdd(fields))
            {
                var template = GetTemplateText(DepositLineTemplate);

                if (template != null)
                {
                    xml = template
                            .Replace("{{Memo}}", fields["Memo"].ToString())
                            .Replace("{{DepositLineEntityFullName}}", fields["DepositLineEntityFullName"].ToString())
                            .Replace("{{FromAccount}}", fields["AccountRef"].ToString())
                            .Replace("{{DepositLineMemo}}", fields["DepositLineMemo"].ToString())
                            .Replace("{{CheckNumber}}", fields["CheckNumber"].ToString())
                            .Replace("{{Amount}}", ((decimal)fields["Amount"]).ToString("F2"));
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

            var envelope = GetTemplateText(EnvelopeTemplate);
            string response = null;

            if(envelope != null)
            {
                response = envelope.Replace("{{Requests}}", xml);
            }

            return response;
        }

        public List<Customer> DeserializeCustomerQueryResponse(string xml)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList customerResponseNodes = xmlDoc.GetElementsByTagName("CustomerRet");

            var response = new List<Customer>();

            if(customerResponseNodes != null)
            { 
                foreach (XmlNode node in customerResponseNodes)
                {
                    var customerTypeNode = node["CustomerTypeRef"];
                 
                    if(node != null && node["FullName"] != null)
                    { 
                        response.Add(new Customer()
                        {
                            FullName = node["FullName"]?.InnerText,
                            CustomerType = customerTypeNode != null && customerTypeNode["FullName"] != null ? customerTypeNode["FullName"]?.InnerText : null
                        });
                    }
                }
            }

            return response;
        }

        public List<PayrollWageItem> DeserializePayrollWageQuery(string xml)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList payrollNodes = xmlDoc.GetElementsByTagName("PayrollItemWageRet");

            var response = new List<PayrollWageItem>();

            foreach (XmlNode node in payrollNodes)
            {
                var expenseAccountNode = node["ExpenseAccountRef"];

                response.Add( new PayrollWageItem()
                { 
                    EditSequence = node["EditSequence"] != null ? long.Parse(node["EditSequence"].InnerText) : 0,
                    Name = node["Name"]?.InnerText,
                    IsActive = bool.Parse(node["IsActive"]?.InnerText), 
                    TimeCreated = DateTime.Parse(node["TimeCreated"]?.InnerText),
                    TimeModified = DateTime.Parse(node["TimeModified"]?.InnerText),
                    ListID = node["ListID"]?.InnerText,    
                    ExpenseAccount = new QuickbooksAccount()
                    {
                        FullName = expenseAccountNode?["FullName"]?.InnerText,
                        ListId = expenseAccountNode?["ListID"]?.InnerText
                    },
                    WageType = node["WageType"]?.InnerText 
                });
            }

            return response;
        }

        public List<QuickbooksAccount> DeserializeAccountQueryResponse(string xml)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);
            XmlNodeList accountResponseNodes = xmlDoc.GetElementsByTagName("AccountRet");
            var response = new List<QuickbooksAccount>();
            foreach (XmlNode node in accountResponseNodes)
            {
                var account = new QuickbooksAccount()
                {
                    ListId = node["ListID"]?.InnerText,
                    FullName = node["FullName"]?.InnerText
                };

                response.Add(account);
            }

            return response; 
        }

        public List<Employee> DeserializeEmployeeQueryResponse(string xml)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList employeeResponseNodes = xmlDoc.GetElementsByTagName("EmployeeRet");

            var response = new List<Employee>();

            foreach (XmlNode node in employeeResponseNodes)
            {
                var payrollNode = node["EmployeePayrollInfo"];

                var employee = new Employee()
                {
                    FirstName = node["FirstName"]?.InnerText,
                    LastName = node["LastName"]?.InnerText,
                    EditSequence = long.Parse(node["EditSequence"]?.InnerText),
                    Name = node["Name"]?.InnerText,
                    IsActive = bool.Parse(node["IsActive"]?.InnerText),
                    PrintAs = node["PrintAs"]?.InnerText,
                    EmployeeType = node["EmployeeType"]?.InnerText,
                    HiredDate = DateTime.Parse(node["HiredDate"]?.InnerText),
                    TimeCreated = DateTime.Parse(node["TimeCreated"]?.InnerText),
                    TimeModified = DateTime.Parse(node["TimeModified"]?.InnerText),
                    ListID = node["ListID"]?.InnerText,
                    PayPeriod = payrollNode?["PayPeriod"]?.InnerText,
                    Earnings = new List<PayrollItem>() 
                };

                if(payrollNode != null)
                { 
                    foreach(XmlNode payrollItem in payrollNode.ChildNodes)
                    {
                        if (payrollItem.Name != "Earnings" || payrollItem["PayrollItemWageRef"] == null)
                            continue;

                        var wageNode = payrollItem["PayrollItemWageRef"];
                     
                        bool isPercent = payrollItem["RatePercent"] != null;

                        employee.Earnings.Add(new PayrollItem()
                        {
                            AmountType = isPercent ? "Percent" : "Amount",
                            Amount = isPercent ? decimal.Parse(payrollItem["RatePercent"]?.InnerText) : decimal.Parse(payrollItem["Rate"]?.InnerText),
                            FullName = wageNode?["FullName"]?.InnerText,
                            ListId = wageNode?["FullName"]?.InnerText,
                            Type = "Wage"
                        }); 
                    }
                }

                response.Add(employee);
            }

            return response;
        }
        public string ConvertAccountQuery(string type)
        {
            var template = GetTemplateText(AccountQueryTemplate);

            string xml = "";

            if (template != null)
            {
                xml = template
                        .Replace("{{AccountType}}", type);
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
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
             
            if(vendorResponseNodes != null)
            {
                foreach (XmlNode node in vendorResponseNodes)
                {
                    var vendorTypeNode = node["VendorTypeRef"];
                    var customFields = new Dictionary<string, string>();

                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "DataExtRet")
                            customFields.Add(child["DataExtName"]?.InnerText, child["DataExtValue"]?.InnerText);
                    }
 
                    response.Add(new Vendor()
                    {
                        Name = node["Name"]?.InnerText,
                        ListId = node["ListID"]?.InnerText,
                        TimeCreated = DateTime.Parse(node["TimeCreated"]?.InnerText),
                        TimeModified = DateTime.Parse(node["TimeModified"]?.InnerText),
                        Type = vendorTypeNode?["FullName"]?.InnerText,
                        Balance = decimal.Parse(node["Balance"]?.InnerText),
                        CustomFields = customFields
                    });
                }
            }
            else
            { 
                throw new Exception("Could not find any Vendors in Quickbooks with the type 'Client'");
            }

            return response;
        }
        public string GetPurchaseOrderXML()
        {
            return GetTemplateText(@"XMLTemplates\TestPurchaseOrderAdd.xml");
        }
        public string ConvertItemQueryByDateRangePrefix(List<string> prefixes, DateTime startDate, DateTime endDate)
        {
            var template = GetTemplateText(ItemQueryDateRangePrefixTemplate); 
            string xml = "";

            if (template != null)
            {
                foreach(var prefix in prefixes)
                {
                    xml += template
                        .Replace("{{StartDate}}", startDate.ToString("yyyy-MM-dd"))
                        .Replace("{{Prefix}}", prefix)
                        .Replace("{{EndDate}}", endDate.ToString("yyyy-MM-dd"));
                } 
            }

            return GetTemplateText(EnvelopeTemplate).Replace("{{Requests}}", xml);
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
                 
                propertyMap["Name"].SetValue(item, node["Name"]?.InnerText);
                propertyMap["Description"].SetValue(item, salesPurchaseNode["Desc"]?.InnerText);
                propertyMap["Price"].SetValue(item, decimal.Parse(salesPurchaseNode["Price"]?.InnerText));

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

        public PurchaseOrderAddResponse ConvertPurchaseOrderAddResponse(string xml)
        {
            var response = new PurchaseOrderAddResponse();

            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList responseNode = xmlDoc.GetElementsByTagName("PurchaseOrderAddRs");

            XmlNodeList purchaseOrderNode = xmlDoc.GetElementsByTagName("PurchaseOrderRet");

            XmlNodeList purchaseOrderLineNodes = xmlDoc.GetElementsByTagName("PurchaseOrderLineRet");

            response.Status = new QuickbooksStatus()
            {
                Code = int.Parse(responseNode[0].Attributes["statusCode"].Value),
                Severity = responseNode[0].Attributes["statusSeverity"].Value,
                Message = responseNode[0].Attributes["statusMessage"].Value
            };

            response.TxnId = purchaseOrderNode[0]["TxnID"]?.InnerText;
            response.TimeCreated = DateTime.Parse(purchaseOrderNode[0]["TimeCreated"]?.InnerText);
            response.TimeModified = DateTime.Parse(purchaseOrderNode[0]["TimeModified"]?.InnerText);
            response.EditSequence = long.Parse(purchaseOrderNode[0]["EditSequence"]?.InnerText);
            response.TxnNumber = int.Parse(purchaseOrderNode[0]["TxnNumber"]?.InnerText);
            response.TxnDate = DateTime.Parse(purchaseOrderNode[0]["TxnDate"]?.InnerText);
 

            response.Lines = new List<PurchaseOrderLine>();
 
            foreach (XmlNode lineNode in purchaseOrderLineNodes)
            {
                response.Lines.Add(new PurchaseOrderLine()
                { 
                    Description = lineNode["Desc"]?.InnerText,
                    Qty = decimal.Parse(lineNode["Quantity"]?.InnerText),
                    Rate = decimal.Parse(lineNode["Rate"]?.InnerText),
                    Item = lineNode["ItemRef"]?["FullName"]?.InnerText,
                    Amount = decimal.Parse(lineNode["Amount"]?.InnerText)
                });

            }

            return response;
        }
        public string ConvertPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            string envelopeXml = GetTemplateText(EnvelopeTemplate);

            var template = GetTemplateText(PurchaseOrderAddTemplate);
             
            string xml = "";

            if(template != null)
            {
                xml = template
                        .Replace("{{VendorName}}", purchaseOrder.VendorName)
                        .Replace("{{LineItems}}", ConvertPurchaseOrderLines(purchaseOrder.Lines)); 
            }
              
            return envelopeXml.Replace("{{Requests}}", xml);
        }
        public string ConvertPurchaseOrderLines(List<PurchaseOrderLine> lines)
        { 
            string addRequests = "";

            foreach (var line in lines)
            {
                addRequests += ConvertPurchaseOrderLine(line);
            }

            return addRequests;
        }


        public string ConvertPurchaseOrderLine(PurchaseOrderLine line)
        {
            var template = GetTemplateText(PurchaseOrderLineAddTemplate);
            string xml = "";

            if(template != null)
            {
                xml = template
                        .Replace("{{ItemName}}", line.Item)
                        .Replace("{{Quantity}}", line.Qty.ToString())
                        .Replace("{{Rate}}", line.Rate.ToString());
            }

            return xml;
        }

        public string ConvertItems<T>(List<T> items)
        {
            string envelopeXml = GetTemplateText(EnvelopeTemplate);
            string addRequests = "";

            foreach (var item in items)
            {
                addRequests += ConvertItem<T>(item);
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
                   fieldValues.ContainsKey("AccountRef") &&
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



        public DepositAddResponse ConvertDepositAddResponse(string xml )
        {
            var response = new DepositAddResponse();

            var xmlDoc = new XmlDocument(); 

            xmlDoc.LoadXml(xml);

            XmlNodeList responseNode = xmlDoc.GetElementsByTagName("DepositAddRs");

            XmlNodeList depositNode = xmlDoc.GetElementsByTagName("DepositRet");
            XmlNodeList depositLineNodes = xmlDoc.GetElementsByTagName("DepositLineRet");

            response.Status = new QuickbooksStatus()
            {
                Code = int.Parse(responseNode[0].Attributes["statusCode"].Value),
                Severity = responseNode[0].Attributes["statusSeverity"].Value,
                Message = responseNode[0].Attributes["statusMessage"].Value
            };

            response.TxnId = depositNode[0]["TxnID"]?.InnerText;
            response.TimeCreated = DateTime.Parse(depositNode[0]["TimeCreated"]?.InnerText);
            response.TimeModified = DateTime.Parse(depositNode[0]["TimeModified"]?.InnerText);
            response.EditSequence = long.Parse(depositNode[0]["EditSequence"]?.InnerText);
            response.TxnNumber = int.Parse(depositNode[0]["TxnNumber"]?.InnerText);
            response.TxnDate = DateTime.Parse(depositNode[0]["TxnDate"]?.InnerText);
            response.Memo = depositNode[0]["Memo"]?.InnerText;
            response.DepositTotal = decimal.Parse(depositNode[0]["DepositTotal"]?.InnerText);

            response.Lines = new List<DepositLine>();

            response.DepositAccount = new QuickbooksAccount()
            {
                ListId = depositNode[0]["DepositToAccountRef"]["ListID"]?.InnerText,
                FullName = depositNode[0]["DepositToAccountRef"]["FullName"]?.InnerText
            };

            foreach(XmlNode lineNode in depositLineNodes)
            {
                response.Lines.Add(new DepositLine() {

                    TxnType = lineNode["TxnType"]?.InnerText,
                    TxnLineId = lineNode["TxnLineID"]?.InnerText,
                    Memo = lineNode["Memo"]?.InnerText,
                    CheckNumber = lineNode["CheckNumber"]?.InnerText,
                    Amount = decimal.Parse(lineNode["Amount"]?.InnerText),
                    Account = new QuickbooksAccount()
                    {
                        ListId = lineNode["AccountRef"]["ListID"]?.InnerText,
                        FullName = lineNode["AccountRef"]["FullName"]?.InnerText
                    },
                    Entity = new QuickbooksEntity()
                    {
                        ListId = lineNode["EntityRef"]["ListID"]?.InnerText,
                        FullName = lineNode["EntityRef"]["FullName"]?.InnerText
                    },
                    Method = new PaymentMethod()
                    {
                        ListId = lineNode["PaymentMethodRef"]["ListID"]?.InnerText,
                        FullName = lineNode["PaymentMethodRef"]["FullName"]?.InnerText
                    } 
                });

            }
  
            return response;
        }

        public DepositQueryResponse ConvertDepositQueryResponse(string xml)
        {
            var response = new DepositQueryResponse();

            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList responseNode = xmlDoc.GetElementsByTagName("DepositQueryRs");

            XmlNodeList depositNodes = xmlDoc.GetElementsByTagName("DepositRet");
            
            response.Status = new QuickbooksStatus()
            {
                Code = int.Parse(responseNode[0].Attributes["statusCode"].Value),
                Severity = responseNode[0].Attributes["statusSeverity"].Value,
                Message = responseNode[0].Attributes["statusMessage"].Value
            };

            response.Deposits = new List<QuickbooksDeposit>();

            foreach(XmlNode depositNode in depositNodes)
            {
                XmlNodeList childNodes = depositNode.ChildNodes; 

                var deposit = new QuickbooksDeposit()
                {
                    TxnId = depositNode["TxnID"]?.InnerText,
                    TimeCreated = DateTime.Parse(depositNode["TimeCreated"]?.InnerText),
                    TimeModified = DateTime.Parse(depositNode["TimeModified"]?.InnerText),
                    EditSequence = long.Parse(depositNode["EditSequence"]?.InnerText),
                    TxnNumber = int.Parse(depositNode["TxnNumber"]?.InnerText),
                    TxnDate = DateTime.Parse(depositNode["TxnDate"]?.InnerText),
                    Memo = depositNode["Memo"]?.InnerText,
                    DepositTotal = decimal.Parse(depositNode["DepositTotal"]?.InnerText),

                    Lines = new List<DepositLine>(),

                    DepositAccount = new QuickbooksAccount()
                    {
                        ListId = depositNode["DepositToAccountRef"]["ListID"]?.InnerText,
                        FullName = depositNode["DepositToAccountRef"]["FullName"]?.InnerText
                    } 
                };

                if(childNodes != null)
                { 
                    foreach (XmlNode lineNode in childNodes )
                    {
                        if (lineNode.Name != "DepositLineRet")
                            continue;

                        if(lineNode["Memo"] == null)
                        {
                            if(lineNode["CheckNumber"] != null)
                                throw new Exception("Error: deposit for check number " + lineNode["CheckNumber"]?.InnerText + " is missing a memo.");
                            else
                                throw new Exception("Error: a deposit without a check number is missing a memo.");
                        }

                        deposit.Lines.Add(new DepositLine()
                        { 
                            TxnType = lineNode["TxnType"]?.InnerText,
                            TxnLineId = lineNode["TxnLineID"]?.InnerText,
                            Memo = lineNode["Memo"]?.InnerText,
                            CheckNumber = lineNode["CheckNumber"]?.InnerText,
                            Amount = decimal.Parse(lineNode["Amount"]?.InnerText),
                            Account = new QuickbooksAccount()
                            {
                                ListId = lineNode["AccountRef"]["ListID"]?.InnerText,
                                FullName = lineNode["AccountRef"]["FullName"]?.InnerText
                            },
                            Entity = new QuickbooksEntity()
                            {
                                ListId = lineNode["EntityRef"]["ListID"]?.InnerText,
                                FullName = lineNode["EntityRef"]["FullName"]?.InnerText
                            },
                            Method = new PaymentMethod()
                            {
                                ListId = lineNode["PaymentMethodRef"]["ListID"]?.InnerText,
                                FullName = lineNode["PaymentMethodRef"]["FullName"]?.InnerText
                            }
                        }); 
                    }
                }
                response.Deposits.Add(deposit);
            }
             
            return response;
        }

        public List<ItemServiceAddResponse> ConvertItemServiceAddResponse(string xml)
        {
            var response = new List<ItemServiceAddResponse>();

            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xml);

            XmlNodeList itemAddResponseNodes = xmlDoc.GetElementsByTagName("ItemServiceAddRs");
              
            foreach (XmlNode itemAddNode in itemAddResponseNodes)
            {
                XmlNode itemNode= itemAddNode["ItemServiceRet"];

                var itemResponse = new ItemServiceAddResponse();
                itemResponse.Status = new QuickbooksStatus()
                {
                    Code = int.Parse(itemAddNode.Attributes["statusCode"].Value),
                    Severity = itemAddNode.Attributes["statusSeverity"].Value,
                    Message = itemAddNode.Attributes["statusMessage"].Value
                };

                itemResponse.ListId = itemNode?["ListID"]?.InnerText;
                itemResponse.TimeCreated = DateTime.Parse(itemNode?["TimeCreated"]?.InnerText);
                itemResponse.TimeModified = DateTime.Parse(itemNode?["TimeModified"]?.InnerText);
                itemResponse.EditSequence = long.Parse(itemNode?["EditSequence"]?.InnerText);
                itemResponse.Name = itemNode?["Name"]?.InnerText;
                itemResponse.FullName = itemNode?["FullName"]?.InnerText;
                itemResponse.IsActive = bool.Parse(itemNode?["IsActive"]?.InnerText);
                itemResponse.Sublevel = int.Parse(itemNode?["Sublevel"]?.InnerText);

                itemResponse.Details = new SalesOrPurchase()
                {
                    Account = new QuickbooksAccount()
                    {
                        ListId = itemNode?["SalesOrPurchase"]?["AccountRef"]["ListID"]?.InnerText,
                        FullName = itemNode?["SalesOrPurchase"]?["AccountRef"]["FullName"]?.InnerText
                    },
                    Description = itemNode?["SalesOrPurchase"]?["Desc"]?.InnerText,
                    Price = decimal.Parse(itemNode?["SalesOrPurchase"]?["Price"]?.InnerText),
                };

                response.Add(itemResponse);
            }

            return response;
        }
    }
}
