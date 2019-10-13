using Ninject;
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

        private IKernel IocKernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IocKernel = new StandardKernel(); // typically a static member
            InjectableUserControl.Kernel = IocKernel;
            IocKernel.Bind<IClientService>().To<TestClientService>();
            IocKernel.Bind<ISalespersonService>().To<TestSalespersonService>();

        }

        private void ConfigureContainer()
        {
         
        }

        private void ComposeObjects()
        {
           
            
        }
    }
}
