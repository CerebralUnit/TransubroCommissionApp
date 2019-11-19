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
                clients.Add(new Client()
                {
                    Name = vendor.Name,
                    ThresholdDate = vendor.CustomFields.ContainsKey("Old Claim Date") ? DateTime.Parse(vendor.CustomFields["Old Claim Date"]) : default(DateTime?),
                    TransubroPercentageNew = decimal.Parse(vendor.CustomFields["New Claim Percent"])/100,
                    TransubroPercentageOld = decimal.Parse(vendor.CustomFields["Old Claim Percent"])/100,
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

            return customers.Where(x => x.CustomerType == "Insurance Company").Select(x => x.FullName).ToList();
        }

        public List<Claim> SearchClaims(List<string> fileNumbers, DateTime startDate, DateTime endDate)
        {
            var qbc = new QuickbooksClient(AppName);
 
            return qbc.SearchItems<Claim>(fileNumbers, startDate, endDate);
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
        public bool ProcessCheckDeposits(List<CheckDeposit> deposits)
        {
            var qbc = new QuickbooksClient(AppName);

            var claims = new List<Claim>();

            var depositResponses = new List<DepositAddResponse>();

            var itemResponses = new List<ItemServiceAddResponse>();

            foreach (var deposit in deposits)
            {
                if (deposit.PropertyDamageAmount > 0)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = deposit.PropertyDamageAmount,
                        Description = deposit.PropertyDamageDescription,
                        FileNumber = deposit.FileNumber + "-PD"
                    });
                }

                if (deposit.LossOfUseAmount > 0)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = deposit.LossOfUseAmount,
                        Description = deposit.LossOfUseDescription,
                        FileNumber = deposit.FileNumber + "-LOU"
                    });
                }


                if (deposit.OtherAmount > 0)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = deposit.OtherAmount,
                        Description = deposit.OtherDescription,
                        FileNumber = deposit.FileNumber + "-OTH"
                    });
                }

                depositResponses.Add(qbc.AddDeposit(deposit));
            }

            itemResponses = qbc.AddItems(claims);
             
            return true;
        } 
    }
}
