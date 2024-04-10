using MassTransit;

namespace TaskProcessor
{
    public class ServiceRunner : BackgroundService
    {
        private readonly IBusControl _busControl;

        public ServiceRunner(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting {GetType().FullName}...");
            _busControl.StartAsync(cancellationToken);
            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            Console.WriteLine("Service has stopped");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
