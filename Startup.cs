using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SDNPortal.Startup))]
namespace SDNPortal
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
