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
        private readonly string companyFile;
        public const string AppName = "Transubro Commissions";

        public QuickbooksService(string companyFileLocation)
        {
            companyFile = companyFileLocation;
        }

        private QuickbooksClient GetClient()
        {
            return new QuickbooksClient(companyFile, AppName);
        }

        public List<Client> GetClients()
        {
            var qbc = GetClient();

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
            var qbc = GetClient();

            return qbc.GetActivePayrollItemsWage();
        }

        public List<string> GetInsuranceCompanies()
        {
            var qbc = GetClient();

            List<Customer> customers = qbc.GetActiveCustomers();

            return customers?.Where(x => x.CustomerType == "Insurance Company").Select(x => x.FullName).ToList();
        }

        public List<Claim> SearchClaims(List<string> fileNumbers, DateTime startDate, DateTime endDate)
        {
            var qbc = GetClient();
 
            return qbc.SearchItems<Claim>(fileNumbers, startDate, endDate);
        }

        public List<QuickbooksAccount> GetIncomeAccounts()
        {
            var qbc = GetClient();

            return qbc.GetAccountsByType("Income");
        }

        public List<Claim> SearchClaims(string client, DateTime startDate, DateTime endDate)
        {
            var qbc = GetClient();

            string responseXML = qbc.SearchItemsByNamePrefixAndDateModified(client, startDate, endDate);

            return new List<Claim>();
        }

        public bool AddClientInvoice(Invoice invoice)
        {  
            var qbc = GetClient();
             
            var response = qbc.AddPurchaseOrder(new PurchaseOrder() {
                VendorName = invoice.ClientName,
                Lines = invoice.Lines.ConvertAll(InvoiceLineToPOLine)
            });
             
            return response.Status.Code == 0;
        }


        public bool AddCommissionInvoice(Invoice invoice)
        {
            var qbc = GetClient();

            var response = qbc.AddPurchaseOrder(new PurchaseOrder()
            {
                VendorName = invoice.ClientName,
                Lines = invoice.Lines.ConvertAll(CommissionInvoiceLineToPOLine)
            });

            return response.Status.Code == 0;
        }

        private PurchaseOrderLine InvoiceLineToPOLine(InvoiceLine line)
        {
            return new PurchaseOrderLine()
            {
                Description = line.Description, 
                Item = line.FileNumber,
                Qty = decimal.Parse(line.CheckAmount.Replace("$", "").Replace(",","")),
                Rate = decimal.Parse(line.SplitRate.Replace("$", "").Replace(",", ""))/100
            };
        }

        private PurchaseOrderLine CommissionInvoiceLineToPOLine(InvoiceLine line)
        {
            PurchaseOrderLine convertedLine = null;
            if (line.IsFlatCommission)
            {
                convertedLine = new PurchaseOrderLine()
                {
                    Description = line.Description,
                    Item = line.FileNumber,
                    Qty = 1,
                    Rate = decimal.Parse(line.CommissionRate)
                };
            }
            else
            {
                convertedLine = new PurchaseOrderLine()
                {
                    Description = line.Description,
                    Item = line.FileNumber,
                    Qty = Math.Round(line.CompanyAmount, 2),
                    Rate = decimal.Parse(line.CommissionRate) / 100
                };
            }
            return convertedLine;
        }

        public string DepositCheck()
        {
            var qbc = GetClient();

            return qbc.DepositCheck();
        }
        public List<QuickbooksDeposit> GetDepositsByDateRange(DateTime startDate, DateTime endDate)
        {
            var qbc = GetClient();

            return qbc.GetDepositsByDateRange(startDate, endDate);
        }

        public List<Employee> SearchEmployees(string name)
        {
            var qbc = GetClient();

            return qbc.SearchEmployeesByName(name);
        }
        public bool ProcessCheckDeposits(List<Check> checks)
        {
            var qbc = GetClient();

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

                if(check.IsClosed)
                {
                    claims.Add(new Claim()
                    {
                        CheckAmount = -3,
                        Description = "Closed file fee",
                        FileNumber = check.FileNumber + "-CLS"
                    }); 
                }
            }
            depositResponses.Add(qbc.AddDeposit(checks));
            itemResponses = qbc.AddItems(claims);
             
            return true;
        } 
    }
}
