namespace TaskProcessor
{
    internal class WarrantyCheckTask
    {
        public async Task ProcessWarrantyCheck()
        {
            Console.WriteLine("Starting the Warranty Check process...");

            await Task.Delay(TimeSpan.FromSeconds(5));

            Console.WriteLine("Warranty Check process completed.");
        }
    }
}
