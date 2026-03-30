using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CaregiverLite.Startup))]
namespace CaregiverLite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Schedular.Main();
            
        }
    }
}
