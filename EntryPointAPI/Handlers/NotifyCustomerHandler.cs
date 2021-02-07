using System;
using System.Threading.Tasks;
using EntryPointAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using OnboardingMessages;
using Rebus.Handlers;
using Serilog;

namespace EntryPointAPI.Handlers
{
    public class NotifyCustomerHandler : IHandleMessages<NotifyServiceDesk>
    {
        private readonly IHubContext<ActivityHub, IActivityHub> _activityHubContext;

        public NotifyCustomerHandler(IHubContext<ActivityHub, IActivityHub> activityHubContext)
        {
            _activityHubContext = activityHubContext ?? throw new ArgumentNullException(nameof(activityHubContext));
        }
        
        public async Task Handle(NotifyServiceDesk m)
        {
            string greeting = $"Dear customer please be aware that that: {m.Message}.";
            Log.Information($"{greeting}.");
            //send the notification to All (Todo: should be to a particular User(connectionID) )
            await _activityHubContext.Clients.All.SendMessage(greeting);
            //return Task.CompletedTask;
        }
    }
}