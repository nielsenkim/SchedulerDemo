using Coravel;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskScheduler;


var builder = Host.CreateApplicationBuilder();

builder.Services.AddScheduler();
builder.Services.AddTransient<WarrantyCheckScheduler>();

builder.Services.AddMassTransit((busConfigurator) =>
{
    busConfigurator.UsingRabbitMq((context, rmqConfig) =>
    {
        MessageBusOptions? options = builder.Configuration.GetSection(MessageBusOptions.MessageBus).Get<MessageBusOptions>();
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


var app = builder.Build();

app.Services.UseScheduler(scheduler => 
{
    scheduler.Schedule<WarrantyCheckScheduler>()
    .EveryTenSeconds()
    .PreventOverlapping(nameof(WarrantyCheckScheduler));
});

app.Run();


public class MessageBusOptions
{
    public static string MessageBus = "MessageBus";

    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Tenant { get; set; } = string.Empty;
}