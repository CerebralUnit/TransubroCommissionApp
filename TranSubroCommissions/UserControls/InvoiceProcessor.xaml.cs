using QBXML.NET;
using QBXML.NET.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TranSubroCommissions.Properties;

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for InvoiceProcessor.xaml
    /// </summary>
    public partial class InvoiceProcessor : InjectableUserControl
    {
        private const string FILENUMBER_SPLITTER = @"^([A-Z]+(?:[~-][A-Z]{1,3})?)-([0-9]{6}(?:-[A-Za-z0-9]+)?)(?:[ \-])?([A-Z]{2,5})?$";

        public enum UpdateType
        {
            Text,
            Alert,
            Title
        }

        public InvoiceProcessor()
        {
            InitializeComponent();
        }

        private void UpdateStatus(string Message, UpdateType type = UpdateType.Text)
        {
            StatusBlock.Dispatcher.Invoke(() => {
                StatusBlock.Text += "\n";
                StatusBlock.Text += Message; 
            });
              
            invoices.Dispatcher.Invoke(() => {

                var text = new TextBlock() { Text = Message };

                if (type == UpdateType.Alert)
                {
                    text.Background = Brushes.LightCyan;
                    text.Padding = new Thickness(5);
                    text.Margin = new Thickness(0, 0, 0, 10);
                    text.HorizontalAlignment = HorizontalAlignment.Stretch;
                    text.TextAlignment = TextAlignment.Left;
                    text.FontWeight = FontWeights.Bold;
                   
                }
                else if(type == UpdateType.Title)
                {
                    text.FontSize = 18;
                    text.Margin = new Thickness(0, 15, 0, 10);
                    text.FontWeight = FontWeights.Bold;
                }
                 
                invoices.Children.Add(text);
            }); 
        }
        private void ResetStatus()
        {
            StatusBlock.Dispatcher.Invoke(() => {
                StatusBlock.Text = String.Empty;
            });
        }
        private void Grid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent?.RaiseEvent(eventArg);
            }
        }
      
        private DataGrid GetInvoiceGrid(string recipientType)
        {
            Style s = new Style();
            s.Setters.Add(new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Right));
            s.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0)));
            s.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.Transparent));

            var splitType = recipientType == "Salesperson" ? "TS" : "Client";

            var grid = new DataGrid();
            grid.PreviewMouseWheel += Grid_PreviewMouseWheel;
            grid.IsReadOnly = true;
            grid.AutoGenerateColumns = false;

            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "#";
            
            textColumn.Binding = new Binding("LineNumber");
            textColumn.Width = 50;

            DataGridTextColumn fileNoColumn = new DataGridTextColumn();
            fileNoColumn.Header = "File #";
            fileNoColumn.Binding = new Binding("FileNumber");
            fileNoColumn.MinWidth = 200;

            DataGridTextColumn descColumn = new DataGridTextColumn();
            descColumn.Header = "Description";
            descColumn.Binding = new Binding("Description");
            descColumn.MinWidth = 220;

            DataGridTextColumn checkAmtColumn = new DataGridTextColumn();
            checkAmtColumn.Header = "Check Amt";
            checkAmtColumn.Binding = new Binding("CheckAmount");
            checkAmtColumn.HeaderStyle = s;
            checkAmtColumn.CellStyle = s;
            checkAmtColumn.MinWidth = 100;

            DataGridTextColumn percentColumn = new DataGridTextColumn();
            percentColumn.Header = splitType + " %";
            percentColumn.Binding = new Binding("SplitRate");
            percentColumn.HeaderStyle = s;
            percentColumn.CellStyle = s;
            percentColumn.MinWidth = 80;
             
            DataGridTextColumn commissionColumn = new DataGridTextColumn();
            commissionColumn.Header = "Sales %";
            commissionColumn.Binding = new Binding("CommissionRate");
            commissionColumn.HeaderStyle = s;
            commissionColumn.CellStyle = s;
            commissionColumn.MinWidth = 80;

            DataGridTextColumn dueColumn = new DataGridTextColumn();
            dueColumn.Header = "Due " + recipientType;
            dueColumn.Binding = new Binding("AmountDue");
            dueColumn.HeaderStyle = s;
            dueColumn.CellStyle = s;
            dueColumn.MinWidth = 150;

            grid.Columns.Add(textColumn);
            grid.Columns.Add(fileNoColumn);
            grid.Columns.Add(descColumn);
            grid.Columns.Add(checkAmtColumn);
            grid.Columns.Add(percentColumn);

            if (recipientType == "Salesperson")
                grid.Columns.Add(commissionColumn);

            grid.Columns.Add(dueColumn);


            return grid;
        }
        private void ShowWarning(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }

        private Dictionary<string, List<DepositLine>> GetChecksByClient(List<DepositLine> checks)
        {
            var response = new Dictionary<string, List<DepositLine>>();

            foreach (var check in checks)
            { 
                string filenumber = "";

                if (String.IsNullOrWhiteSpace(check.Memo))
                {
                    filenumber = "NO MEMO";
                }
                else
                {
                    filenumber = ParseFileNumber(check.Memo).FirstOrDefault() ?? "NO MEMO";
                }
                 
                if (!response.ContainsKey(filenumber))
                    response.Add(filenumber, new List<DepositLine>());

                response[filenumber].Add(check);
            }

            return response;
        }

        private Dictionary<string, List<Claim>> GetClaimsByClient(List<Claim> claims)
        {
            var response = new Dictionary<string, List<Claim>>();
            
            foreach (var claim in claims)
            {
                var parsed = new List<string>();
                string filenumber = "";

                if (String.IsNullOrWhiteSpace(claim.FileNumber))
                {
                    filenumber = "NO FILENUMBER"; 
                }
                else
                {
                    filenumber = ParseFileNumber(claim.FileNumber).FirstOrDefault() ?? "NO FILENUMBER";
                }
                
                if (!response.ContainsKey(filenumber))
                    response.Add(filenumber, new List<Claim>());

                response[filenumber].Add(claim);
            }

            return response;
        }

        private string GetFilenumberFromLineItem(string memo)
        {
            if (String.IsNullOrWhiteSpace(memo))
                return "NO MEMO";

            var parsed = ParseFileNumber(memo);

            if (parsed.Count == 0)
                return "NO MEMO";

            return parsed[0] + "-" + parsed[1];
        }

        private void ProcessInvoices(DateTime? startDate, DateTime? endDate)
        {
            invoices.Dispatcher.Invoke(() =>
            {
                invoices.Children.Clear();
            });

            ResetStatus();
          
            if (startDate.HasValue && endDate.HasValue)
            {
                var qbc = new QuickbooksClient(Settings.Default.CompanyFile, QuickbooksService.AppName);
                var qb = new QuickbooksService(Settings.Default.CompanyFile);

                UpdateStatus("Gathering deposits for " + startDate.Value.ToString("MM/dd/yy") + " to " + endDate.Value.ToString("MM/dd/yy"), UpdateType.Alert);
          
                List<QuickbooksDeposit> deposits = qb.GetDepositsByDateRange(startDate.Value, endDate.Value);
                List<DepositLine> checks = deposits.SelectMany(x => x.Lines).ToList();
                 
                var checksByClient = GetChecksByClient(checks);
                 
                if (checksByClient.Any(x => x.Key == "NO MEMO")) 
                    ShowWarning("One or more deposits was missing a required Memo Line. The memo should be formatted CLIENT-DATE TYPE (e.g. TRM-042419 LOU)"); 
                 
                UpdateStatus("Gathering clients", UpdateType.Alert);

                List<string> clientNames = checksByClient.Keys.Distinct().ToList();

                Dictionary<string, Client> clients = qb.GetClients().Where(x => clientNames.Contains(x.Name)).ToDictionary(x => x.Name, x => x);


                List<string> fileNumbers = checks.Select(x =>
                    GetFilenumberFromLineItem(x.Memo)
                 )
                .Distinct()
                .ToList();
                 
                UpdateStatus("Retrieving items to create invoice lines", UpdateType.Alert); 

                List<Claim> claims = qb.SearchClaims(fileNumbers, startDate.Value, endDate.Value);
                
                Dictionary<string, List<Claim>> claimsByClient = GetClaimsByClient(claims);

                if (claimsByClient.Any(x => x.Key == "NO FILENUMBER")) 
                    ShowWarning("One or more claim items had an improperly formatted name."); 

                UpdateStatus("Retrieving salesperson commission list", UpdateType.Alert); 

                List<Employee> salespersons = qbc.SearchEmployeesByName("{salesperson}");

                Dictionary<string, PayrollWageItem> commissionItems = qb.GetActivePayrollItemsWage().Where(x => x.WageType == "Commission").ToDictionary(x => x.Name, x => x);
 
                UpdateStatus("Building client invoices", UpdateType.Alert); 

                foreach (var client in clients)
                {
                    DataGrid datagrid = null;
                    
                    var invoiceLines = new List<InvoiceLine>();
                   
                    if (!claimsByClient.ContainsKey(client.Key))
                    {
                        UpdateStatus("No deposits were found for " + client.Key, UpdateType.Alert); 
                        continue;
                    }
                    else
                    { 
                        UpdateStatus("Client Invoice for " + client.Key + "", UpdateType.Title); 
                    }
                     
                    invoices.Dispatcher.Invoke(() => {
                        datagrid = GetInvoiceGrid("Client");
                        datagrid.Name = client.Key + "GridClientInvoices";
                        invoices.Children.Add(datagrid);
                        datagrid.ItemsSource = invoiceLines;
                    });
                     
                    var invoice = new ClientInvoice()
                    {
                        Client = client.Key,
                        Claims = claimsByClient[client.Key]
                    };
                     
                    int line = 1;
                    decimal invoiceTotal = 0;
                    decimal clientDueTotal = 0;

                    foreach (var claim in invoice.Claims)
                    {
                        decimal clientPercent = 0;
                        decimal dueClient = 0;

                        if (claim.CheckAmount < 0)
                        {
                            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                            culture.NumberFormat.CurrencyNegativePattern = 1;

                            dueClient = claim.CheckAmount;
                            invoiceLines.Add(new InvoiceLine()
                            {
                                LineNumber = line.ToString(),
                                FileNumber = "Disbursement",
                                Description = claim.Description,
                                CheckAmount = String.Format(culture, "{0:C}", dueClient),
                                AmountDue = String.Format(culture, "{0:C}", dueClient),
                                SplitRate = "",
                                CommissionRate = ""
                            });
                        }
                        else
                        {
                            clientPercent = GetClientPercentForCheck(claim.FileNumber, client.Value);
                            dueClient = clientPercent * claim.CheckAmount;

                            invoiceLines.Add(new InvoiceLine()
                            {
                                LineNumber = line.ToString(),
                                FileNumber = claim.FileNumber,
                                Description = claim.Description,
                                CheckAmount = claim.CheckAmount.ToString("c"),
                                AmountDue = dueClient.ToString("c"),
                                SplitRate = (clientPercent * 100).ToString("#.000"),
                                CommissionRate = ""
                            });

                        }
                         
                        clientDueTotal += dueClient;
                        invoiceTotal += claim.CheckAmount;
                        line++;
                    }
                     
                    string items = invoice.Claims.Count > 1 ? "items" : "item";

                    qb.AddClientInvoice(new Invoice() {
                        ClientName = client.Key,
                        Lines = new List<InvoiceLine>(invoiceLines)
                    });

                    invoiceLines.Add(new InvoiceLine()
                    {
                        LineNumber = "",
                        FileNumber = "",
                        Description = "",
                        CheckAmount = "",
                        SplitRate = "",
                        CommissionRate = "",
                        AmountDue = ""
                    });
                   
                    invoiceLines.Add(new InvoiceLine()
                    {
                        LineNumber = "",
                        FileNumber = "",
                        Description = "",
                        CheckAmount = "",
                        SplitRate = "",
                        CommissionRate = "Total Client Due",
                        AmountDue = clientDueTotal.ToString("c")
                    });

                    datagrid.Dispatcher.Invoke(() => {
                        datagrid.Items.Refresh();
                    });
                  
                    UpdateStatus(" ");
                } 

                UpdateStatus("Client invoices complete.", UpdateType.Alert); 
                UpdateStatus("Building salesperson invoices", UpdateType.Alert); 

                foreach (var salesperson in salespersons)
                {
                    DataGrid datagrid = null;
                    var invoiceLines = new List<InvoiceLine>();
                    
                    int line = 1;
   
                    decimal invoiceTotal = 0;
                    decimal salesDueTotal = 0;
                     
                    foreach (var commission in salesperson.Earnings)
                    {
                        if(claimsByClient.ContainsKey(commission.FullName))
                        { 
                            UpdateStatus("Commission invoice for " + salesperson.Name, UpdateType.Title);
                            invoices.Dispatcher.Invoke(() => {
                                datagrid = GetInvoiceGrid("Salesperson");
                                invoices.Children.Add(datagrid);
                                datagrid.ItemsSource = invoiceLines;
                            });

                           

                           

                            var clientClaims = claimsByClient[commission.FullName];

                            foreach(var clientClaim in clientClaims)
                            {
                                if (clientClaim.CheckAmount < 0)
                                    continue;

                                decimal companyPercent = GetCompanyPercentForCheck(clientClaim.FileNumber, clients[commission.FullName]);
                                decimal companyAmount = companyPercent * clientClaim.CheckAmount;
                                decimal salesPersonDue = (commission.Amount/100) * companyAmount;

                                if (commission.AmountType == "Amount")
                                    salesPersonDue = commission.Amount;

                                invoiceLines.Add(new InvoiceLine()
                                {
                                    LineNumber = line.ToString(),
                                    FileNumber = clientClaim.FileNumber,
                                    Description = clientClaim.Description,
                                    CheckAmount = clientClaim.CheckAmount.ToString("c"),
                                    SplitRate = (companyPercent * 100).ToString("#.000"),
                                    CommissionRate = commission.Amount.ToString("#.000"),
                                    AmountDue = salesPersonDue.ToString("c"),
                                    CompanyAmount = companyPercent * clientClaim.CheckAmount,
                                    IsFlatCommission = commission.AmountType == "Amount"
                                });
 
                                salesDueTotal += salesPersonDue;
                                invoiceTotal += clientClaim.CheckAmount;
                                line++; 
                            }
                        }
                    }

                    if(invoiceLines.Count > 0)
                    { 
                        qb.AddCommissionInvoice(new Invoice()
                        {
                            ClientName = salesperson.Name.Replace("{salesperson}", "").Trim() + " - COMMISSION",
                            Lines = new List<InvoiceLine>(invoiceLines)
                        });


                        invoiceLines.Add(new InvoiceLine()
                        {
                            LineNumber = "",
                            FileNumber = "",
                            Description = "",
                            CheckAmount = "",
                            AmountDue = ""
                        });

                        invoiceLines.Add(new InvoiceLine()
                        {
                            LineNumber = "",
                            FileNumber = "",
                            Description = "",
                            CheckAmount = "Total Commissions",
                            AmountDue = salesDueTotal.ToString("c")
                        });
                        datagrid.Dispatcher.Invoke(() => {
                            datagrid.Items.Refresh();
                        });
                    }

                    

                   

                }
            }
        }

        private List<string> ParseFileNumber(string filenumber)
        {
            var matches = Regex.Match(filenumber, FILENUMBER_SPLITTER);
            var parsed = new List<string>();

            for(var i = 1; i < matches.Groups.Count; i++)
            {
                parsed.Add(matches.Groups[i].Value);
            }

            return parsed;
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
            var parsed = ParseFileNumber(fileNumber);
            decimal commission = client.ClientPercentageNew;

            if(parsed != null && parsed.Count > 1)
            {
                string dateString = parsed[1];
                DateTime date = DateTime.MaxValue;
                var dashIndex = dateString.IndexOf("-");

                if (dashIndex > -1) 
                    dateString = dateString.Substring(0, dashIndex); 

                try
                {
                    date = DateTime.ParseExact(dateString, "MMddyy", CultureInfo.InvariantCulture); 
                }
                catch
                {
                    throw new FormatException("The date portion of the claim item was formatted incorrectly, should be MMddyy, but instead was " + dateString + " in item " + fileNumber);
                }

                if (date < client.ThresholdDate)
                    commission = client.ClientPercentageOld;
            }
            else
            {
                throw new FormatException("The date portion of the claim item was missing in item " + fileNumber);
            }

            return commission;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDate.SelectedDate;
            DateTime? endDate = EndDate.SelectedDate;

            await Task.Run(() => { 
                try
                {
                    ProcessInvoices(startDate, endDate);
                }
                catch(Exception ex)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(ex.Message, "Failed to process invoices", MessageBoxButton.OK, MessageBoxImage.Error); 
                    });
                }
            });
        }
    }
}
