using Ninject;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddClaims.xaml
    /// </summary>
    public partial class AddClaims : InjectableUserControl
    {
        [Inject]
        public IClientService clients { private get; set; }
        public List<CheckDeposit> claims = new List<CheckDeposit>() { new CheckDeposit() };

        public List<string> InsuranceCompanies { get; set; }
        public AddClaims()
        {
            InitializeComponent();
            var qbService = new QuickbooksService();
            InsuranceCompanies = qbService.GetInsuranceCompanies();


            this.DataContext = this;
            this.Loaded += delegate
            {
                //clientDropdown.ItemsSource = clients.GetAllClients();
                ClaimChecks.ItemsSource = claims;

                
            };
        }

        private void AddCheckButton_Click(object sender, RoutedEventArgs e)
        {
            claims.Add(new CheckDeposit());
            ClaimChecks.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCheck_Click(object sender, RoutedEventArgs e)
        {
            var claim = ((Button)sender).DataContext as CheckDeposit;

            if (claims.Count > 1)
            { 
                claims.Remove(claim);
                try { 
                    ClaimChecks.Items.Refresh();
                }
                catch { }
            }
        }

        private void CheckSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; 
            int claimCount = claims.Count;
            List<CheckDeposit> emptyClaims = new List<CheckDeposit>();
            foreach (var claim in claims)
            {

                if ( 
                   String.IsNullOrWhiteSpace(claim.FileNumber) &&
                   String.IsNullOrWhiteSpace(claim.From) &&
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
                    String.IsNullOrWhiteSpace(claim.From) ||
                    (String.IsNullOrWhiteSpace(claim.LossOfUseDescription) && String.IsNullOrWhiteSpace(claim.PropertyDamageDescription)) ||
                    String.IsNullOrWhiteSpace(claim.Memo) ||
                    String.IsNullOrWhiteSpace(claim.CheckNumber) ||
                    claim.CheckAmount == 0)

                {
                    isValid = false;
                    break;
                }
            }

            List<CheckDeposit> validClaims = claims.Where(x => !emptyClaims.Contains(x)).ToList();

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
                 
                bool processClaimsSucceeded = new QuickbooksService().ProcessCheckDeposits(claims);

                if(processClaimsSucceeded)
                { 
                    claims.Clear();

                    claims.Add(new CheckDeposit());

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

        private void PDDesc_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
