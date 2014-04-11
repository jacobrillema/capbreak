using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Capbreak.Startup))]
namespace Capbreak
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
