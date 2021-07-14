using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderItemsReserver
{
    internal class OrderListDto
    {
        [Required]
        [ValidateEachItem]
        public IList<OrderItemDto> Items { get; set; }
    }
}
