using System.Collections.Generic;

namespace Microsoft.eShopWeb.Infrastructure.Services.ServiceBusOrderSender
{
    internal class OrderListDto
    {
        public OrderListDto()
        {
            Items = new List<OrderItemDto>();
        }

        public IList<OrderItemDto> Items { get; set; }

        public string ShippingAddress { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
