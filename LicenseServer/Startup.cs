using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LicenseServer.Startup))]
namespace LicenseServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
