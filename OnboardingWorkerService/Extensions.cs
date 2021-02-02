using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingMessages;
using Rebus.Auditing.Messages;
using Rebus.Config;
using Rebus.Persistence.FileSystem;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.FileSystem;

namespace OnboardingWorkerService
{
    public static class Extensions
    {
        public static string BackplaneConnectionString =
            "Server=localhost; Database=ServiceBus; User=sa; Password=secret!;";
        public static void AddRebusAsSendAndReceive(this IServiceCollection services, IConfiguration config)
        {
            services.AddRebus(
                rebus => rebus
                   .Logging(x   => x.Serilog())
                   .Routing(x   => x.TypeBased().MapAssemblyOf<OnboardNewCustomer>("MainQueue"))
                   .Transport(x => x.UseFileSystem("c:/rebus-advent", "MainQueue"))
                   .Options(x   => x.SimpleRetryStrategy(errorQueueAddress: "ErrorQueue"))
                   .Options(x   => x.EnableMessageAuditing(auditQueue: "AuditQueue"))
                   .Sagas(x     => x.UseFilesystem("c:/rebus-advent/sagas"))
                   .Timeouts(x  => x.UseFileSystem("c:/rebus-advent/timeouts"))
            );

            services.AutoRegisterHandlersFromAssemblyOf<RebusHostedService>();
        }

        public static void AddRebusAsSendAndReceiveUsingSqlServer(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddRebus(
                rebus => rebus
                    .Logging(x => x.Serilog())
                    .Transport(x => x.UseSqlServer(BackplaneConnectionString, "Messages"))
                    .Routing(r => r.TypeBased()
                        .MapAssemblyOf<OnboardNewCustomer>("Messages"))
                    .Sagas(x =>
                    {
                        x.StoreInSqlServer(
                            connectionString: BackplaneConnectionString,
                            dataTableName: "sagas",
                            indexTableName: "SagasIndex");
                    })
            );
            
            services.AutoRegisterHandlersFromAssemblyOf<RebusHostedService>();
        }
    }
}