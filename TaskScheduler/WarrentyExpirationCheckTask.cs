using Coravel.Invocable;
using MassTransit;
using Messages;

namespace TaskScheduler
{
    internal class WarrantyCheckScheduler : IInvocable
    {
        private readonly IBus bus;

        public WarrantyCheckScheduler(IBus bus)
        {
            this.bus = bus;
        }
        public async Task Invoke()
        {
            Console.WriteLine("Invoke WarrantyCheckScheduler...");
            await bus.Publish(new WarrantyCheckMessage());
        }
    }
}
