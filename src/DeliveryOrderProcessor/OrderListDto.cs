using DeliveryOrderProcessor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryOrderProcessor
{
    internal class OrderListDto
    {
        [Required]
        [ValidateEachItem]
        public IList<OrderItemDto> Items { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public decimal FinalPrice { get; set; }
    }
}
