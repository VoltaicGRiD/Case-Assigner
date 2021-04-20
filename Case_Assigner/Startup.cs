using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Case_Assigner.Startup))]

namespace Case_Assigner
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}
