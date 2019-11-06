using QBXML.NET;
using QBXML.NET.Model;
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
    /// Interaction logic for InvoiceProcessor.xaml
    /// </summary>
    public partial class InvoiceProcessor : InjectableUserControl
    {
        public InvoiceProcessor()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //Get all deposits in date range
            //Get all items that match the file numbers in the deposits
            //Get all of the clients based on the items
            //Get all of the salespeople for the clients
            var qbc = new QuickbooksClient(QuickbooksService.AppName);
            var qb = new QuickbooksService();

            DateTime? startDate = StartDate.SelectedDate;
            DateTime? endDate = EndDate.SelectedDate;

            if(startDate.HasValue && endDate.HasValue)
            { 
                List<QuickbooksDeposit> deposits = qb.GetDepositsByDateRange(startDate.Value, endDate.Value);

                List<string> clientNames = deposits.Select(x => x.Memo.Substring(0, x.Memo.IndexOf("-"))).Distinct().ToList();

                List<Client> clients = qb.GetClients().Where(x => clientNames.Contains(x.Name)).ToList();
                  
                List<string> fileNumbers = deposits.Select(x => x.Memo.Substring(0, x.Memo.IndexOf(" "))).Distinct().ToList();

                List<Claim> items = qb.SearchClaims(fileNumbers, startDate.Value, endDate.Value);

                List<Employee> salespeople = new QuickbooksClient(QuickbooksService.AppName).SearchEmployeesByName("{salesperson}");

                Dictionary<string, PayrollWageItem> commissionItems = qb.GetActivePayrollItemsWage().Where(x => x.WageType == "Commission").ToDictionary(x => x.Name, x => x);

                
            } 
        }
    }
}
