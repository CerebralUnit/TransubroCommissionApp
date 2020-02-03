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
using TranSubroCommissions.Properties;

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for ClientList.xaml
    /// </summary>
    public partial class SalespersonList : InjectableUserControl
    { 
        public SalespersonList( )
        { 
            InitializeComponent();
            this.Loaded += delegate
            {
                SalespersonsList.ItemsSource =  new QuickbooksService(Settings.Default.CompanyFile).SearchEmployees("{salesperson}");
            };
        }

        
    }
}
