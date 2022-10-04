using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DatePurchaced { get; set; }

        public DateTime DateDelivered { get; set; }

        public bool IsDelivered { get; set; }

        public Category? Category { get; set; }

        public int? CategoryId { get; set; }

        public Product? Product { get; set; }

        public int? ProductId { get; set; }

        public User? User { get; set; }

        public int? UserId { get; set; }

    }
}
