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
using System.Windows.Forms;
using TranSubroCommissions.Properties;

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsScreen : InjectableUserControl
    {
        public SettingsScreen()
        {
            InitializeComponent();

            fileLocation.Text = Settings.Default.CompanyFile;
        }

        private void directorySelect_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Quickbooks Company Files (*.qbw)|*.qbw|All files (*.*)|*.*";
            dialog.ValidateNames = false;
             
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string sFileName = dialog.FileName;
                fileLocation.Text = sFileName;

                Settings.Default.CompanyFile = sFileName;
                Settings.Default.Save();
            } 
        }
    }
}
