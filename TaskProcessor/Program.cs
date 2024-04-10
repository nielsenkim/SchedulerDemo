using MassTransit;

namespace TaskProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<ServiceRunner>();
            builder.Services.AddTransient<WarrantyCheckTask>();
            builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

            var host = builder.Build();
            host.Run();
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit((busConfigurator) =>
            {
                busConfigurator.AddConsumer<WarrantyCheckConsumer>();

                busConfigurator.UsingRabbitMq((context, rmqConfig) =>
                {
                    MessageBusOptions? options = configuration.GetSection(MessageBusOptions.MessageBus).Get<MessageBusOptions>();
                    if (options == null)
                    {
                        throw new NullReferenceException(nameof(options));
                    }

                    rmqConfig.Host(options.Host, options.Tenant, hostConfigurator =>
                    {
                        hostConfigurator.Username(options.Username);
                        hostConfigurator.Password(options.Password);
                    });

                    rmqConfig.ConfigureEndpoints(context);
                });
            });
        }
    }

}

public class MessageBusOptions
{
    public static string MessageBus = "MessageBus";

    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Tenant { get; set; } = string.Empty;
}