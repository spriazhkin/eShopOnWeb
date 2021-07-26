using System.ComponentModel.DataAnnotations;

namespace DeliveryOrderProcessor
{
    internal class OrderItemDto
    {
        [Required]
        public string ItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive count allowed")]
        public int? Count { get; set; }
    }
}
