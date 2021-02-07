using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OnboardingMessages;
using Rebus.Handlers;
using Serilog;

namespace EntryPointAPI.Hubs
{
    public class ActivityHub : Hub //<IActivityHub>
    ,IHandleMessages<WelcomeEmailSent>
    {
        public ActivityHub()
        {
          //  _eventHubServer = eventHubServer ?? throw new ArgumentNullException(nameof(eventHubServer));
        }
        
       // private readonly IHubContext<ActivityHub> _eventHubServer;
        public async Task SendMessage(string message)
        {
           // await _eventHubServer.Clients.All.SendAsync("data-feed", message);
           await Clients.All.SendAsync("data-feed", message);
        }
        
        public override Task OnConnectedAsync()
        {
            Log.Information($"Connected from  : {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        
        async Task IHandleMessages<WelcomeEmailSent>.Handle(WelcomeEmailSent domainEvent)
        {
            Log.Information("Handling WelcomeEmailSent sent event");
            await SendMessage("Welcome Mail sent");
        }
        
    }
}