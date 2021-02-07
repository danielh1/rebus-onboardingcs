using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OnboardingMessages;
using Rebus.Handlers;
using Serilog;

namespace EntryPointAPI.Hubs
{
    public class ActivityHub : Hub<IActivityHub>
   // ,IHandleMessages<WelcomeEmailSent>
    {
        private readonly IHubContext<ActivityHub> _activityHub;
        public ActivityHub(IHubContext<ActivityHub> activityHub)
        {
          _activityHub = activityHub ?? throw new ArgumentNullException(nameof(activityHub));
        }
        
         
        public async Task SendMessage(string message)
        {
           // await _eventHubServer.Clients.All.SendAsync("data-feed", message);
           await _activityHub.Clients.All.SendAsync("data-feed", message);
           //await Clients.User(userConnectionId).SendAsync("account_registration_result", message);
        }
        
     
        
        public override Task OnConnectedAsync()
        {
            Log.Information($"Connected from  : {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        
        // async Task IHandleMessages<WelcomeEmailSent>.Handle(WelcomeEmailSent domainEvent)
        // {
        //     Log.Information("Handling WelcomeEmailSent sent event");
        //     await SendMessage("Welcome Mail sent");
        // }
        
    }
}