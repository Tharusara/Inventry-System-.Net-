using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERPEC.Startup))]
namespace ERPEC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
