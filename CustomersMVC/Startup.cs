using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomersMVC.Startup))]
namespace CustomersMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
