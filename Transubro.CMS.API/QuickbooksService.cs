using Interop.QBFC13;
using QBXML.NET;
using QBXML.NET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transubro.CMS.Model;

namespace Transubro.CMS.API
{
    public class QuickbooksService
    {
        public const string AppName = "Transubro Commissions";

        public List<Client> GetClients()
        {
            var qbc = new QuickbooksClient(AppName);

            List<Vendor> vendors = qbc.GetVendorsByType("Client");
            List<Client> clients = new List<Client>();

            foreach (var vendor in vendors)
            {
                if( vendor.CustomFields == null || 
                    !vendor.CustomFields.ContainsKey("New Claim Percent") ||
                    !vendor.CustomFields.ContainsKey("Old Claim Percent")
                 )
                {
                    throw new Exception("The vendor with name " + vendor.Name + " is configured incorrectly. Please make sure that the follow custom fields are set in Quickbooks: 'New Claim Percent', and 'Old Claim Percent'");

                }

                clients.Add(new Client()
                {
                    Name = vendor.Name.ToUpper(),
                    ThresholdDate = vendor.CustomFields.ContainsKey("Old Claim Date") ? DateTime.Parse(vendor.CustomFields["Old Claim Date"]) : default(DateTime?),
                    ClientPercentageNew = decimal.Parse(vendor.CustomFields["New Claim Percent"])/100,
                    ClientPercentageOld = decimal.Parse(vendor.CustomFields["Old Claim Percent"])/100,
                });
            }

            return clients;
        }

        public List<PayrollWageItem> GetActivePayrollItemsWage()
        {
            var qbc = new QuickbooksClient(AppName);

            return qbc.GetActivePayrollItemsWage();
        }

        public List<string> GetInsuranceCompanies()
        {
            var qbc = new QuickbooksClient(AppName);

            List<Customer> customers = qbc.GetActiveCustomers();

            return customers?.Where(x => x.CustomerType == "Insurance Company").Select(x => x.FullName).ToList();
        }

        public List<Claim> SearchClaims(List<string> fileNumbers, DateTime startDate, DateTime endDate)
        {
            var qbc = new QuickbooksClient(AppName);
 
            return qbc.SearchItems<Claim>(fileNumbers, startDate, endDate);
        }

        public List<QuickbooksAccount> GetIncomeAccounts()
        {
            var qbc = new QuickbooksClient(AppName);

            return qbc.GetAccountsByType("Income");
        }

        public List<Claim> SearchClaims(string client, DateTime startDate, DateTime endDate)
        {
            var qbc = new QuickbooksClient(AppName);

            string responseXML = qbc.SearchItemsByNamePrefixAndDateModified(client, startDate, endDate);

            return new List<Claim>();
        }

        public string DepositCheck()
        {
            var qbc = new QuickbooksClient(AppName);

            return qbc.DepositCheck();
        }
        public List<QuickbooksDeposit> GetDepositsByDateRange(DateTime startDate, DateTime endDate)
        {
            var qbc = new QuickbooksClient(AppName);

            return qbc.GetDepositsByDateRange(startDate, endDate);
        }

        public List<Employee> SearchEmployees(string name)
        {
            var qbc = new QuickbooksClient(AppName);

            return qbc.SearchEmployeesByName(name);
        }
        public bool ProcessCheckDeposits(List<Check> checks)
        {
            var qbc = new QuickbooksClient(AppName);

            var claims = new List<Claim>();

            var depositResponses = new List<DepositAddResponse>();

            var itemResponses = new List<ItemServiceAddResponse>();

            var deposit = new Deposit(checks);

            foreach (var check in checks)
            {
                if (check.PropertyDamageAmount > 0)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = check.PropertyDamageAmount.Value,
                        Description = check.PropertyDamageDescription,
                        FileNumber = check.FileNumber + "-PD"
                    });
                }

                if (check.LossOfUseAmount > 0)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = check.LossOfUseAmount.Value,
                        Description = check.LossOfUseDescription,
                        FileNumber = check.FileNumber + "-LOU"
                    });
                }


                if (check.OtherAmount > 0)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = check.OtherAmount.Value,
                        Description = check.OtherDescription,
                        FileNumber = check.FileNumber + "-OTH"
                    });
                } 
            }
            depositResponses.Add(qbc.AddDeposit(checks));
            itemResponses = qbc.AddItems(claims);
             
            return true;
        } 
    }
}
