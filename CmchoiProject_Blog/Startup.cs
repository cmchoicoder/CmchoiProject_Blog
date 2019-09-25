using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CmchoiProject_Blog.Startup))]
namespace CmchoiProject_Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
