namespace Microsoft.eShopWeb.Infrastructure.Services.ServiceBusOrderSender
{
    public class ServiceBusOrderSenderConfiguration
    {
        public string ServiceBusConnectionString { get; set; }
        
        public string QueueName { get; set; }
    }
}
