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
using QBXML.NET;
namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for NewClientScreen.xaml
    /// </summary>
    public partial class NewClientScreen : InjectableUserControl
    {
        [Inject]
        public ISalespersonService sales { private get; set; }
        public List<SalesCommission> commissions = new List<SalesCommission>()
        {
                 new SalesCommission()
                 {
                    
                 }
        };

        public List<SalesPerson> SalesPersons { get; set; }
        public NewClientScreen()
        {
            InitializeComponent();
            SalesPersons = sales.GetAllSalesPersons();
            CommissionsList.ItemsSource = commissions;
            this.DataContext = this; 
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var commission = ((ComboBox)sender).DataContext as SalesCommission;
            var salesperson = ((ComboBox)sender).SelectedItem as SalesPerson;

            commission.SalesPersonName = salesperson.Name;
        }

        private void NewCommission_Click(object sender, RoutedEventArgs e)
        {
            var qbService = new QuickbooksService();
            qbService.GetClients();
            var qbc = new QuickbooksClient("Transubro Commissions");
            //var items = qbc.SearchItems<Claim>("ACE", new DateTime(2019, 10, 14), new DateTime(2019, 12, 15)); 
            //var vendors = qbService.GetClients(); 
            //var salesPeople = qbc.GetVendorsByType ("Salesperson");
            //var deposit = qbc.DepositCheck ();
            var payrollItems = qbc.GetActivePayrollItemsWage();

            var employee = qbc.SearchEmployeesByName("{salesperson}");
            var deposits = qbc.GetDepositsByDateRange(new DateTime(2019, 10, 20), new DateTime(2019, 12, 21));

            var insuranceCompanies = qbService.GetInsuranceCompanies();
            //qbc.AddItems(new List<object>()
            //{
            //    new Claim() { CheckAmount = 55.52m, Description = "This is a test, man", FileNumber = "MXT-12354-3333" },
            //    new Claim() { CheckAmount = 155.11m, Description = "Another Item Here", FileNumber = "ACE-115522-1234" },
            //    new Claim() { CheckAmount = 229.00m, Description = "I have a claim for you", FileNumber = "PT-042219" }
            //});

            // qbc.AddPurchaseOrder();
            commissions.Add(new SalesCommission());
            CommissionsList.Items.Refresh();
            
        }

        private void DeleteCommission_Click(object sender, RoutedEventArgs e)
        {
            var commission = ((Button)sender).DataContext as SalesCommission;

            if(commissions.Count > 1)
            {
                commissions.Remove(commission);
            }
            else
            {
                commissions[0].Amount = null;
                commissions[0].SalesPersonName = null;
            }
            CommissionsList.Items.Refresh();

        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var commission = ((TextBox)sender).DataContext as SalesCommission;
            var amount = ((TextBox)sender).Text;
            decimal parsedAmount;
            if(decimal.TryParse(amount, out parsedAmount))
            {
                commission.Amount = parsedAmount;

            }
            else
            {
                commission.Amount = null;
            }
        }
    }
}
