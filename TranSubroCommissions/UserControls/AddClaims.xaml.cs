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
        public List<Claim> claims = new List<Claim>() { new Claim() };
        public AddClaims()
        {
            InitializeComponent();
            this.Loaded += delegate
            {
                clientDropdown.ItemsSource = clients.GetAllClients();
                ClaimChecks.ItemsSource = claims;
            };
        }

        private void AddCheckButton_Click(object sender, RoutedEventArgs e)
        {
            claims.Add(new Claim());
            ClaimChecks.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCheck_Click(object sender, RoutedEventArgs e)
        {
            var claim = ((Button)sender).DataContext as Claim;

            claims.Remove(claim);
            ClaimChecks.Items.Refresh();
        }
    }
}
