using System.Collections.Generic;

namespace Microsoft.eShopWeb.Infrastructure.Services
{
    internal class OrderListDto
    {
        public OrderListDto()
        {
            Items = new List<OrderItemDto>();
        }

        public IList<OrderItemDto> Items { get; set; }
    }
}
