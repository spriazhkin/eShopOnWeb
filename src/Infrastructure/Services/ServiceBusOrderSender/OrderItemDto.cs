namespace Microsoft.eShopWeb.Infrastructure.Services.ServiceBusOrderSender
{
    internal class OrderItemDto
    {
        public string ItemId { get; set; }

        public int? Count { get; set; }
    }
}
