using QBXML.NET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Transubro.CMS.API;

namespace TranSubroCommissions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        protected override void OnStartup(StartupEventArgs e)
        {
           // base.OnStartup(e);
            var splashScreen = new Splashscreen("Connecting to Quickbooks");
            this.MainWindow = splashScreen;
            splashScreen.Show();

            Task.Factory.StartNew(() =>
            {
                string errorMessage = null;

                try
                {
                    //simulate some work being done
                    new QuickbooksClient(QuickbooksService.AppName);
                }
                catch(Exception ex)
                {
                    errorMessage = ex.Message;
                }

                //since we're not on the UI thread
                //once we're done we need to use the Dispatcher
                //to create and show the main window
                this.Dispatcher.Invoke(() =>
                { 
                    if(errorMessage == null)
                    { 
                        this.MainWindow = new MainWindow();
                        this.MainWindow.Show();
                        splashScreen.Close();
                    }
                    else
                    {
                        MessageBox.Show("This application cannot run while Quickbooks is open. Please close all instances of Quickbooks on this computer and try again: Error Message - " + errorMessage,
                            "Quickbooks Connection Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        Current.Shutdown();
                    }
                });
            }); 
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            var splashScreen = new Splashscreen("Closing Quickbooks");
            this.MainWindow = splashScreen;

            new QuickbooksClient().Disconnect();

            splashScreen.Close();
        }
    }
}
