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

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for ClientList.xaml
    /// </summary>
    public partial class ClientList : UserControl
    {

        private List<Client> clients = new List<Client>()
        {
            new Client()
            {
                Name = "TXW",
                ThresholdDate = new DateTime(2014, 4, 13),
                TransubroPercentageNew = 0.58m,
                TransubroPercentageOld = 0.4m
            },
            new Client()
            {
                Name = "DT",
                ThresholdDate = new DateTime(2011, 1, 11),
                TransubroPercentageNew = 0.68m,
                TransubroPercentageOld = 0.54m
            }
        };

        public ClientList()
        {
            InitializeComponent();
            CurrentClients.ItemsSource = clients;
        }
    }
}
