﻿using QBXML.NET;
using QBXML.NET.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Transubro.CMS.API;
using Transubro.CMS.Model;

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for InvoiceProcessor.xaml
    /// </summary>
    public partial class InvoiceProcessor : InjectableUserControl
    {
        public InvoiceProcessor()
        {
            InitializeComponent();
        }
        private void UpdateStatus(string Message)
        {
            StatusBlock.Dispatcher.Invoke(() => {
                StatusBlock.Text += "\n";
                StatusBlock.Text += Message;
            });
        }

        private void ProcessInvoices(DateTime? startDate, DateTime? endDate)
        {
            //Get all deposits in date range
            //Get all items that match the file numbers in the deposits
            //Get all of the clients based on the items
            //Get all of the salespeople for the clients
           

            

            if (startDate.HasValue && endDate.HasValue)
            {
                var qbc = new QuickbooksClient(QuickbooksService.AppName);
                var qb = new QuickbooksService();

                UpdateStatus("Getting deposits from between dates");
                List<QuickbooksDeposit> deposits = qb.GetDepositsByDateRange(startDate.Value, endDate.Value);

                Dictionary<string, List<QuickbooksDeposit>> depositsByClient = deposits
                    .GroupBy(x =>
                        x.Memo.Substring(0, x.Memo.IndexOf("-"))
                    )
                    .ToDictionary(x => x.Key, x => x.ToList());


                UpdateStatus("Gathering clients");
                List<string> clientNames = deposits.Select(x => x.Memo.Substring(0, x.Memo.IndexOf("-"))).Distinct().ToList();

                Dictionary<string, Client> clients = qb.GetClients().Where(x => clientNames.Contains(x.Name)).ToDictionary(x => x.Name, x => x);


                List<string> fileNumbers = deposits.Select(x => x.Memo.Substring(0, x.Memo.IndexOf(" "))).Distinct().ToList();


                UpdateStatus("Retrieving items to create invoice lines");
                List<Claim> claims = qb.SearchClaims(fileNumbers, startDate.Value, endDate.Value);

                Dictionary<string, List<Claim>> claimsByClient = claims
                    .GroupBy(x =>
                        x.FileNumber.Substring(0, x.FileNumber.IndexOf("-"))
                    )
                    .ToDictionary(x => x.Key, x => x.ToList());


                UpdateStatus("Retrieving salesperson commission list");
                List<Employee> salespersons = qbc.SearchEmployeesByName("{salesperson}");

                Dictionary<string, PayrollWageItem> commissionItems = qb.GetActivePayrollItemsWage().Where(x => x.WageType == "Commission").ToDictionary(x => x.Name, x => x);
 
                UpdateStatus("Building client invoices");
                foreach (var client in clients)
                { 
                    if (!claimsByClient.ContainsKey(client.Key))
                    {
                        UpdateStatus("No deposits were found for " + client.Key);
                        continue;
                    }
                    else
                    {

                        UpdateStatus("_______________________________________________________________________");
                        UpdateStatus("Building invoice for " + client.Key + "");
                    }

                    var invoice = new ClientInvoice()
                    {
                        Client = client.Key,
                        Claims = claimsByClient[client.Key]
                    };

                    UpdateStatus("");
                    UpdateStatus("Invoice Lines");

                    int line = 1;
                    decimal invoiceTotal = 0;
                    decimal clientDueTotal = 0;

                    foreach (var claim in invoice.Claims)
                    {
                        decimal clientPercent = GetClientPercentForCheck(claim.FileNumber, client.Value);
                        decimal dueClient = clientPercent * claim.CheckAmount;

                        UpdateStatus(line + ". " + claim.FileNumber + "....." + claim.Description + "....." + claim.CheckAmount.ToString("c") + "....." + dueClient.ToString("c") + " Due Client");

                        clientDueTotal += dueClient;
                        invoiceTotal += claim.CheckAmount;
                        line++;
                    }

                    string items = invoice.Claims.Count > 1 ? "items" : "item";
                    UpdateStatus("Total: " + invoiceTotal.ToString("c"));
                    UpdateStatus("Client Due: " + clientDueTotal.ToString("c"));
                    UpdateStatus("Invoice complete");
                    //Build client invoice
                }

                UpdateStatus("Client invoices complete.");

                UpdateStatus("Building salesperson invoices");

                foreach (var salesperson in salespersons)
                {
                    int line = 1;
                    UpdateStatus("Building commission invoice for " + salesperson.Name);
                    decimal invoiceTotal = 0;
                    decimal salesDueTotal = 0;
                    foreach(var commission in salesperson.Earnings)
                    {
                        if(claimsByClient.ContainsKey(commission.FullName))
                        {
                            var clientClaims = claimsByClient[commission.FullName];

                            foreach(var clientClaim in clientClaims)
                            {
                                decimal companyPercent = GetCompanyPercentForCheck(clientClaim.FileNumber, clients[commission.FullName]);
                                decimal companyAmount = companyPercent * clientClaim.CheckAmount;
                                decimal salesPersonDue = (commission.Amount/100) * companyAmount;

                                UpdateStatus(line + ". " + clientClaim.FileNumber + "....." + clientClaim.Description + "....." + clientClaim.CheckAmount.ToString("c") + "....." + salesPersonDue.ToString("c") + " Commission Due");

                                salesDueTotal += salesPersonDue;
                                invoiceTotal += clientClaim.CheckAmount;
                                line++; 
                            }
                        }
                    } 
                }
            }
        }
        private decimal GetCompanyPercentForCheck(string fileNumber, Client client)
        {
            string[] fileNumberSplit = fileNumber.Split('-');
            decimal commission = client.TransubroPercentageNew;

            if (fileNumberSplit.Length > 1)
            {
                string dateString = fileNumberSplit[1];

                DateTime date = DateTime.ParseExact(dateString, "MMddyy", CultureInfo.InvariantCulture);

                if (date < client.ThresholdDate)
                    commission = client.TransubroPercentageOld;
            }

            return commission;
        }
        private decimal GetClientPercentForCheck(string fileNumber, Client client)
        {
            string[] fileNumberSplit = fileNumber.Split('-');
            decimal commission = client.ClientPercentageNew;

            if(fileNumberSplit.Length > 1)
            {
                string dateString = fileNumberSplit[1];

                DateTime date = DateTime.ParseExact(dateString, "MMddyy", CultureInfo.InvariantCulture);
                 
                if (date < client.ThresholdDate)
                    commission = client.ClientPercentageOld;
            }

            return commission;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDate.SelectedDate;
            DateTime? endDate = EndDate.SelectedDate;

            await Task.Run(() => ProcessInvoices(startDate, endDate));
        }
    }
}
