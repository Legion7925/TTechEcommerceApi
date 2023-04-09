using EcommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace TTechEcommerceApi.Model
{
    public class OrderUpdateModel
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DatePurchaced { get; set; }

        public DateTime DateDelivered { get; set; }

        public bool IsDelivered { get; set; }

        public int? CategoryId { get; set; }

        public int? ProductId { get; set; }

        public int? UserId { get; set; }

    }
}
