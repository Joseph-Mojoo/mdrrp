using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mdrrp_prototype.Startup))]
namespace mdrrp_prototype
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
