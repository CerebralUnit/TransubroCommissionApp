using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Interop.QBXMLRP2;
using QBXML.NET.Model;

namespace QBXML.NET
{
    public class QuickbooksClient
    {
        private string appName;
        private const string companyFile = @"C:\Users\Annie\Documents\Intuit\Quickbooks\VM\Company Files\transubro, inc..qbw";

        protected static RequestProcessor2 MyQbXMLRP2;
        private static string ticket = null;

        public QuickbooksClient() { }

        public QuickbooksClient(string applicationName)
        {
            appName = applicationName;

            if(MyQbXMLRP2 == null)
            {
                MyQbXMLRP2 = new RequestProcessor2(); 
                MyQbXMLRP2.OpenConnection2("", appName, QBXMLRPConnectionType.localQBD); 
            }
            
            if(ticket == null)
            {
                try
                { 
                    ticket = MyQbXMLRP2.BeginSession(companyFile, QBFileMode.qbFileOpenDoNotCare); 
                }
                catch (COMException ex)
                {
                    throw ex;
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        public void Disconnect()
        {
            if (ticket != null && MyQbXMLRP2 != null) 
                MyQbXMLRP2.EndSession(ticket);  

            if(MyQbXMLRP2 != null)
                MyQbXMLRP2.CloseConnection();

            MyQbXMLRP2 = null;
            ticket = null;
        }

        public string ProcessRequest(string xml)
        {  
            // The variable “xmlRequestSet” in the following line represents a fully formed qbXML request set;
            //This snippet omitted the code that assembled the request set in order to keep the
            //example focused on the session and the connection.   
            string sendXMLtoQB = MyQbXMLRP2.ProcessRequest(ticket, xml);

           

            return sendXMLtoQB;
        }

        public List<PayrollWageItem> GetActivePayrollItemsWage()
        {
            var converter = new QBXMLConverter();

            var xml = converter.GetPayrollItemQuery();

            string responseXml = ProcessRequest(xml);

            List<PayrollWageItem> payrollItems = converter.DeserializePayrollWageQuery(responseXml);
             
            return payrollItems;
        }

        public List<Customer> GetActiveCustomers()
        {
            var converter = new QBXMLConverter();

            string xml = converter.GetCustomersQuery();
            List<Customer> customers = null;

            if (xml != null)
            {
                string responseXml = ProcessRequest(xml);
                customers = converter.DeserializeCustomerQueryResponse(responseXml);
            }
           
            return customers;
        }
        public List<Employee> SearchEmployeesByName(string name)
        {
            var converter = new QBXMLConverter();

            string xml = converter.ConvertEmployeeQuery(name);

            string responseXml = ProcessRequest(xml);

            List<Employee> employees = converter.DeserializeEmployeeQueryResponse(responseXml);

            return employees;
        }

        public string SearchItemsByDateModified(DateTime startDate, DateTime endDate)
        {
            var xml = new QBXMLConverter().ConvertItemQueryByDateRange(startDate, endDate);

            return ProcessRequest(xml);
        }

        public List<Vendor> GetVendorsByType(string vendorType)
        {
            var converter = new QBXMLConverter();

            var xml = converter.ConvertVendorQuery(vendorType);

            var responseXml = ProcessRequest(xml);

            List<Vendor> vendors = converter.DeserializeVendorQueryResponse(responseXml);

            List<Vendor> filteredVendors = vendors.Where(x => x.Type == vendorType).ToList();

            return filteredVendors;
        }
        public string DepositCheck()
        { 
            var xml = new QBXMLConverter().ConvertDeposit();

            return ProcessRequest(xml);
        }
        public List<QuickbooksDeposit> GetDepositsByDateRange(DateTime startDate, DateTime endDate)
        {
            var converter = new QBXMLConverter();

            string xml = converter.ConvertDepositQuery(startDate, endDate);
          
            string queryResponseXml = ProcessRequest(xml);

            DepositQueryResponse response = converter.ConvertDepositQueryResponse(queryResponseXml);

            var results = new List<QuickbooksDeposit>();

            if (response.Status.Code == 0) 
                results = response.Deposits; 

            return results; 
        }

        public string SearchItemsByNamePrefixAndDateModified(List<string> prefix, DateTime startDate, DateTime endDate)
        {
            var converter = new QBXMLConverter();

            var xml = converter.ConvertItemQueryByDateRangePrefix(prefix, startDate, endDate);

            return ProcessRequest(xml);
        }

        public string SearchItemsByNamePrefixAndDateModified(string prefix, DateTime startDate, DateTime endDate)
        {
            var xml = new QBXMLConverter().ConvertItemQueryByDateRangePrefix(prefix, startDate, endDate);

            return ProcessRequest(xml);
        }

        public List<T> SearchItems<T>(List<string> prefix, DateTime startDate, DateTime endDate) where T : new()
        {
            var converter = new QBXMLConverter();

            var requestXml = converter.ConvertItemQueryByDateRangePrefix(prefix, startDate, endDate);

            string responseXml = ProcessRequest(requestXml);

            var items = converter.DeserializeItemQueryResponse<T>(responseXml);

            return items;
        }

        public List<T> SearchItems<T>(string prefix, DateTime startDate, DateTime endDate) where T:new()
        {
            var converter = new QBXMLConverter();

            var requestXml = converter.ConvertItemQueryByDateRangePrefix(prefix, startDate, endDate);

            string responseXml = ProcessRequest(requestXml);

            var items = converter.DeserializeItemQueryResponse<T>(responseXml);
             
            return items;
        }
        public string AddPurchaseOrder()
        {
            var xml = new QBXMLConverter().GetPurchaseOrderXML();


            return ProcessRequest(xml);
        }
        public string AddItem(object item)
        {
            var converter = new QBXMLConverter();

            string xml = converter.ConvertItem(item);

            return ProcessRequest(xml);
        }
        public DepositAddResponse AddDeposit<T>(List<T> checks)
        {
            var converter = new QBXMLConverter();

            string xml = converter.ConvertDeposit(checks);

            string depositResponseXml = ProcessRequest(xml);

            DepositAddResponse response = converter.ConvertDepositAddResponse(depositResponseXml);

            return response;
        }
        
        public List<ItemServiceAddResponse> AddItems<T>(List<T> items)
        {
            var converter = new QBXMLConverter();

            string xml = converter.ConvertItems(items);

            string itemAddResponseXml = ProcessRequest(xml);

            List<ItemServiceAddResponse> response = converter.ConvertItemServiceAddResponse(itemAddResponseXml);

            return response;
        }
    }
}
