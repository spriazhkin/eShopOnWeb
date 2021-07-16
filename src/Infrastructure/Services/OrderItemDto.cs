using System.ComponentModel.DataAnnotations;

namespace Microsoft.eShopWeb.Infrastructure.Services
{
    internal class OrderItemDto
    {
        public string ItemId { get; set; }

        public int? Count { get; set; }
    }
}
