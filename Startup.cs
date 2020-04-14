using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EWebStore.Startup))]
namespace EWebStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
