using MassTransit;
using Messages;

namespace TaskProcessor
{
    internal class WarrantyCheckConsumer : IConsumer<WarrantyCheckMessage>
    {
        private readonly WarrantyCheckTask _warrantyCheckTask;

        public WarrantyCheckConsumer(WarrantyCheckTask warrantyCheckTask)
        {
            _warrantyCheckTask = warrantyCheckTask;
        }

        public async Task Consume(ConsumeContext<WarrantyCheckMessage> context)
        {
            await _warrantyCheckTask.ProcessWarrantyCheck();

        }
    }
}
