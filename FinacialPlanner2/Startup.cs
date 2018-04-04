using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinacialPlanner2.Startup))]
namespace FinacialPlanner2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
