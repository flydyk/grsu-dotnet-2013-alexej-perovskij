using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyCompanySellInfo.Startup))]
namespace MyCompanySellInfo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
