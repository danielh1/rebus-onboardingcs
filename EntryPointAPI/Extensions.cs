using EntryPointAPI.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingMessages;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.FileSystem;
using Rebus.Config;

namespace EntryPointAPI
{
    public static class Extensions
    {
        public static string BackplaneConnectionString =
            "Server=localhost; Database=ServiceBus; User=sa; Password=secret!;";
        public static void AddRebusAsOneWayClient(this IServiceCollection services, IConfiguration config)
        {
            services.AddRebus(
                rebus => rebus
                   .Logging(l => l.Console())
                   .Routing(r => r.TypeBased().Map<OnboardNewCustomer>("MainQueue"))
                   .Transport(t => t.UseFileSystemAsOneWayClient("c:/rebus-advent"))
                   .Options(t => t.SimpleRetryStrategy(errorQueueAddress: "ErrorQueue")));
        }

        public static void AddRebusAsSendAndReceiveUsingSqlServer(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddRebus(
                rebus => rebus
                    .Logging(l => l.Console())
                    .Routing(r =>
                    {
                        r.TypeBased()
                            .Map<OnboardNewCustomer>("Messages")
                            .Map<WelcomeEmailSent>("Messages");
                    })
                    .Transport(x => x.UseSqlServerAsOneWayClient(BackplaneConnectionString))
                    .Options(t => t.SimpleRetryStrategy(errorQueueAddress: "ErrorQueue")));
        }
    }
}