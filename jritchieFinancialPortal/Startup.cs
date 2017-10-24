using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jritchieFinancialPortal.Startup))]
namespace jritchieFinancialPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
