using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TranSubroCommissions
{
    public class InjectableUserControl : UserControl
    {
        // Generally this is considered to be a bad practice, 
        //however I didn't find any better way. If you do, please share :)
        public static IKernel Kernel { private get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            RequestActivation(Kernel);
           
        }

        protected virtual void RequestActivation(IKernel kernel)
        {
            kernel?.Inject(this);
        }
    }
}
