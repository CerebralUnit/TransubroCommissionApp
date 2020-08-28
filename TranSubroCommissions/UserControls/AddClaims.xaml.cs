using Ninject;
using QBXML.NET.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AddClaims.xaml
    /// </summary>
    public partial class AddClaims : InjectableUserControl
    { 
        //public List<Check> claims = new List<Check>() { new Check() };

        public List<Check> claims = new List<Check>(){
            //new Check()
            //{

            //    CheckNumber = "0005017326",
            //    FileNumber = "STA-PA-111219-191162699",
            //    FromAccount = "STA-PA (Insurance Income:STA-PA)",
            //    LossOfUseAmount = 1500.000m,
            //    LossOfUseDescription = "Loss of use at 100% for 5 days", 
            //    OtherAmount = null,
            //    OtherDescription = null,
            //    PropertyDamageAmount = null,
            //    PropertyDamageDescription = null,
            //    ReceivedFrom = "Motorists Insurance",
            //    To = null,
            //    IsClosed = true

            //},
            //new Check()
            //    {

            //    CheckNumber = "320008828",
            //    FileNumber = "MPC-021119-3390",
            //    FromAccount = "MPC (Insurance Income:MPC)",
            //    LossOfUseAmount = 1561.64m,
            //    LossOfUseDescription = "Loss of use at 80% for 3 days", 
            //    OtherAmount = null,
            //    OtherDescription = null,
            //    PropertyDamageAmount = 1000m,
            //    PropertyDamageDescription = "Property damage at 10%",
            //    ReceivedFrom = "American Country",
            //    To = null,
            //    IsClosed = true
            //},
            //new Check()
            //    {

            //    CheckNumber = "0407602217",
            //    FileNumber = "41-010920-63",
            //    FromAccount = "41 (Insurance Income:41)",
            //    LossOfUseAmount = null,
            //    LossOfUseDescription = null, 
            //    OtherAmount = 1087.50m,
            //    OtherDescription = "Arbitration settlement",
            //    PropertyDamageAmount = 2800m,
            //    PropertyDamageDescription = "Property damage at 50%",
            //    ReceivedFrom = "National General",
            //    To = null, 
            //},
            //new Check()
            //    {

            //    CheckNumber = "1000526204",
            //    FileNumber = "YCC-102819",
            //    FromAccount = "YCC     Yellow Cab of Columbus (Insurance Income:YCC     Yellow Cab of Columbus)",
            //    LossOfUseAmount = 371.44m,
            //    LossOfUseDescription = "Loss of use for 30 days at 100%", 
            //    OtherAmount = null,
            //    OtherDescription = null,
            //    PropertyDamageAmount = null,
            //    PropertyDamageDescription = null,
            //    ReceivedFrom = "National General",
            //    To = null, 
            //    IsClosed = true
            //} 
        };


        public static List<string> InsuranceCompanies { get; set; }
        public static List<string> IncomeAccounts { get; set; }

        private bool insuranceDropdownOpen = false;
        private bool incomeDropdownOpen = false;

        public AddClaims()
        {            
            InitializeComponent();
             
            this.DataContext = this;
            ClaimChecks.ItemsSource = claims;
          
            if(InsuranceCompanies == null)
                InsuranceCompanies = new List<string>() { "CONNECTING TO QUICKBOOKS... " };

            if (IncomeAccounts == null)
                IncomeAccounts = new List<string>() { "CONNECTING TO QUICKBOOKS... " };

            this.Loaded += delegate
            {
                if (InsuranceCompanies.Count == 1)
                    LoadInsuranceAccounts();

                if (IncomeAccounts.Count == 1) 
                    LoadIncomeAccounts();

                
            };
        }

        private void LoadInsuranceAccounts()
        {
            Task.Run(() => {
                try
                {
                    var qbService = new QuickbooksService(Settings.Default.CompanyFile);

                    InsuranceCompanies = qbService.GetInsuranceCompanies();

                    Task.Run(() =>
                    {
                        ClaimChecks.Dispatcher.Invoke(() =>
                        {
                            ClaimChecks.ItemsSource = claims;
                            ClaimChecks.Items.Refresh();
                        });
                    });
                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                     //Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("There was an error when trying to retrieve insurance companies: " + ex.Message + " - " + ex.TargetSite + " - line# " + line,
                         "Quickbooks Error",
                         MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    });
                }
            });
        }

        private void LoadIncomeAccounts()
        {
            Task.Run(() => {
                try
                {
                    var qbService = new QuickbooksService(Settings.Default.CompanyFile);

                    var incomeAccounts = qbService.GetIncomeAccounts();

                    if (incomeAccounts != null) 
                        IncomeAccounts = incomeAccounts.Select(x => x.FullName.Replace("Insurance Income:", "") + " (" + x.FullName + ")").ToList();

                    Task.Run(() =>
                    {
                        ClaimChecks.Dispatcher.Invoke(() =>
                        {
                            ClaimChecks.ItemsSource = claims;
                            ClaimChecks.Items.Refresh();
                        });
                    });
                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
               
                    var frame = st.GetFrame(0);
       
                    var line = frame.GetFileLineNumber(); 
                }
            });
        }

        private void AddCheckButton_Click(object sender, RoutedEventArgs e)
        {
            claims.Add(new Check());
            ClaimChecks.Items.Refresh();
        }

        private void RefreshAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadInsuranceAccounts();
            LoadIncomeAccounts();  
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCheck_Click(object sender, RoutedEventArgs e)
        {
            var claim = ((Button)sender).DataContext as Check;

            if (claims.Count > 1)
            { 
                claims.Remove(claim);
                try { 
                    ClaimChecks.Items.Refresh();
                }
                catch { }
            }
        }
        private const string UNWRAP_INSURANCE_PATTERN = @"\([A-Za-z0-9 :\-,]+\)";
        private void CheckSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; 
            int claimCount = claims.Count;
            List<Check> emptyClaims = new List<Check>();
            foreach (var claim in claims)
            {

                if ( 
                   String.IsNullOrWhiteSpace(claim.FileNumber) &&
                   String.IsNullOrWhiteSpace(claim.ReceivedFrom) &&
                   String.IsNullOrWhiteSpace(claim.LossOfUseDescription) &&
                   String.IsNullOrWhiteSpace(claim.PropertyDamageDescription) &&
                   String.IsNullOrWhiteSpace(claim.Memo) &&
                   String.IsNullOrWhiteSpace(claim.CheckNumber) &&
                   claim.CheckAmount == 0)
                {
                    emptyClaims.Add(claim);
                    continue;
                }

                if (
               
                    String.IsNullOrWhiteSpace(claim.FileNumber) ||
                    String.IsNullOrWhiteSpace(claim.ReceivedFrom) ||
                    (String.IsNullOrWhiteSpace(claim.LossOfUseDescription) && String.IsNullOrWhiteSpace(claim.PropertyDamageDescription) && String.IsNullOrWhiteSpace(claim.OtherDescription)) ||
                    String.IsNullOrWhiteSpace(claim.Memo) ||
                    String.IsNullOrWhiteSpace(claim.CheckNumber) ||
                    claim.CheckAmount == 0)

                {
                    isValid = false;
                    break;
                }
            }


            List<Check> validClaims = claims.Where(x => !emptyClaims.Contains(x)).ToList();

            foreach (var claim in validClaims)
            {
                var match = Regex.Match(claim.FromAccount, UNWRAP_INSURANCE_PATTERN);
                try
                {
                    if (match.Groups.Count > 0)
                        claim.FromAccount = match.Groups[0].Value.Substring(1, match.Groups[0].Value.Length - 2);
                }
                catch { }
                
            }
            if (validClaims.Count == 0 || !isValid)
            {
                MessageBoxResult result = MessageBox.Show("Please fill out all of the fields before submitting",
                                          "Incomplete Deposits",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Warning);
            }
            else
            {
                int numberAdded = validClaims.Count;
                 
                bool processClaimsSucceeded = new QuickbooksService(Settings.Default.CompanyFile).ProcessCheckDeposits(claims);

                if(processClaimsSucceeded)
                { 
                    claims.Clear();

                    claims.Add(new Check());

                    ClaimChecks.Items.Refresh();

                    string message = numberAdded + " claims were successfully added to Quickbooks.";

                    if (numberAdded == 1) 
                        message = numberAdded + " claim was successfully added to Quickbooks.";
                     
                    MessageBoxResult result = MessageBox.Show(message,
                                             "Success",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Information); 
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Something went wrong.",
                                            "Oops",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Information);
                }
            }
        }

        private void ClaimChecks_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            
            if (!e.Handled && !insuranceDropdownOpen && !incomeDropdownOpen)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent?.RaiseEvent(eventArg);
            }
        }

        private void PDDesc_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void currency_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            int decimalIndex = txtBox.Text.IndexOf(".");

            if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                if (decimalIndex > -1)
                {
                    e.Handled = true;
                    txtBox.CaretIndex = decimalIndex + 1;
                    return;
                }
            }
            else if (e.Key == Key.Back)
            {
                if (decimalIndex > -1 && txtBox.CaretIndex == decimalIndex + 1)
                {
                    e.Handled = true;
                    txtBox.CaretIndex = decimalIndex;
                    return;
                }
            }
            
        }

        private void currency_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            int decimalIndex = txtBox.Text.IndexOf(".");

            if (txtBox.Text == "$0.00" && txtBox.CaretIndex == 1)
            {
                string val = e.Text;

                txtBox.Text = String.Format("${0}.00", val);
                txtBox.CaretIndex = 2;
                e.Handled = true;
                return;
            }
        }

        private void InsuranceCompaniesCombo_DropDownOpened(object sender, EventArgs e)
        {
            insuranceDropdownOpen = true;
        }

        private void IncomeAccountsCombo_DropDownOpened(object sender, EventArgs e)
        {
            incomeDropdownOpen = true;
        }

        private void IncomeAccountsCombo_DropDownClosed(object sender, EventArgs e)
        {
            incomeDropdownOpen = false;
        }

        private void InsuranceCompaniesCombo_DropDownClosed(object sender, EventArgs e)
        {
            insuranceDropdownOpen = false;
        }
    }
}
