using QBXML.NET;
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
        public class InvoiceLine
        {
            public string LineNumber { get; set; }
            public string FileNumber { get; set; }
            public string Description { get; set; }
            public string CheckAmount { get; set; }
            public string AmountDue { get; set; }
        }
        private DataGrid GetInvoiceGrid(string recipientType)
        {
            Style s = new Style();
            s.Setters.Add(new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Right));
            s.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0)));
            s.Setters.Add(new Setter(DataGridCell.BackgroundProperty, Brushes.Transparent));


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
            descColumn.MinWidth = 300;
            DataGridTextColumn checkAmtColumn = new DataGridTextColumn();
            checkAmtColumn.Header = "Check Amt";
            checkAmtColumn.Binding = new Binding("CheckAmount");
            checkAmtColumn.HeaderStyle = s;
            checkAmtColumn.CellStyle = s;
            checkAmtColumn.MinWidth = 150;
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
            grid.Columns.Add(dueColumn);


            return grid;
        }

        private void ProcessInvoices(DateTime? startDate, DateTime? endDate)
        {
            ResetStatus();
            //Get all deposits in date range
            //Get all items that match the file numbers in the deposits
            //Get all of the clients based on the items
            //Get all of the salespeople for the clients

            if (startDate.HasValue && endDate.HasValue)
            {
                var qbc = new QuickbooksClient(QuickbooksService.AppName);
                var qb = new QuickbooksService();

                UpdateStatus("Gathering deposits for " + startDate.Value.ToString("MM/dd/yy") + " to " + endDate.Value.ToString("MM/dd/yy"), UpdateType.Alert);
          
                List<QuickbooksDeposit> deposits = qb.GetDepositsByDateRange(startDate.Value, endDate.Value);

                Dictionary<string, List<QuickbooksDeposit>> depositsByClient = deposits
                    .GroupBy(x =>
                       x.Memo.IndexOf("-") > -1 ? x.Memo.Substring(0, x.Memo.IndexOf("-")) : x.Memo
                    )
                    .ToDictionary(x => x.Key, x => x.ToList());
                 
                UpdateStatus("Gathering clients", UpdateType.Alert);

                List<string> clientNames = deposits.Select(x =>
                    x.Memo.IndexOf("-") > -1 ? x.Memo.Substring(0, x.Memo.IndexOf("-")) : x.Memo
                ).Distinct().ToList();

                Dictionary<string, Client> clients = qb.GetClients().Where(x => clientNames.Contains(x.Name)).ToDictionary(x => x.Name, x => x);


                List<string> fileNumbers = deposits.Select(x =>
                   x.Memo.IndexOf(" ") > -1 ? x.Memo.Substring(0, x.Memo.IndexOf(" ")) : x.Memo
                 )
                .Distinct()
                .ToList();
                 
                UpdateStatus("Retrieving items to create invoice lines", UpdateType.Alert); 

                List<Claim> claims = qb.SearchClaims(fileNumbers, startDate.Value, endDate.Value);

                Dictionary<string, List<Claim>> claimsByClient = claims
                    .GroupBy(x =>
                        x.FileNumber.IndexOf("-") > -1 ? x.FileNumber.Substring(0, x.FileNumber.IndexOf("-")) : x.FileNumber
                    )
                    .ToDictionary(x => x.Key, x => x.ToList());
                 
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
                        decimal clientPercent = GetClientPercentForCheck(claim.FileNumber, client.Value);
                        decimal dueClient = clientPercent * claim.CheckAmount;
                        
                        invoiceLines.Add(new InvoiceLine()
                        {
                            LineNumber = line.ToString(),
                            FileNumber = claim.FileNumber,
                            Description = claim.Description,
                            CheckAmount = claim.CheckAmount.ToString("c"),
                            AmountDue = dueClient.ToString("c")
                        });
 
                        clientDueTotal += dueClient;
                        invoiceTotal += claim.CheckAmount;
                        line++;
                    }
                     
                    string items = invoice.Claims.Count > 1 ? "items" : "item";

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
                        CheckAmount = "Total Client Due",
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
 
                    UpdateStatus("Commission invoice for " + salesperson.Name, UpdateType.Title);
                     
                    decimal invoiceTotal = 0;
                    decimal salesDueTotal = 0;

                    invoices.Dispatcher.Invoke(() => {
                        datagrid = GetInvoiceGrid("Salesperson");
                        invoices.Children.Add(datagrid);
                        datagrid.ItemsSource = invoiceLines;
                    });

                     
                    foreach (var commission in salesperson.Earnings)
                    {
                        if(claimsByClient.ContainsKey(commission.FullName))
                        {
                            var clientClaims = claimsByClient[commission.FullName];

                            foreach(var clientClaim in clientClaims)
                            {
                                decimal companyPercent = GetCompanyPercentForCheck(clientClaim.FileNumber, clients[commission.FullName]);
                                decimal companyAmount = companyPercent * clientClaim.CheckAmount;
                                decimal salesPersonDue = (commission.Amount/100) * companyAmount;

                                invoiceLines.Add(new InvoiceLine()
                                {
                                    LineNumber = line.ToString(),
                                    FileNumber = clientClaim.FileNumber,
                                    Description = clientClaim.Description,
                                    CheckAmount = clientClaim.CheckAmount.ToString("c"),
                                    AmountDue = salesPersonDue.ToString("c")
                                });
 
                                salesDueTotal += salesPersonDue;
                                invoiceTotal += clientClaim.CheckAmount;
                                line++; 
                            }
                        }
                    }
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
                DateTime date = DateTime.MaxValue;

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
