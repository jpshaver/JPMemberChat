using JPShaver.Modules.JPMemberChat.Components;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("ProductionConfiguration", typeof(JPShaver.Modules.JPMemberChat.Components.Startup))]

namespace JPShaver.Modules.JPMemberChat.Components
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}