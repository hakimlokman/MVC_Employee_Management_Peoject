using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Employee_Management_Peoject.Startup))]
namespace Employee_Management_Peoject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
