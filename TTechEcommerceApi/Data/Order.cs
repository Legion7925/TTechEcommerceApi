using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTechEcommerceApi.Data;

namespace EcommerceApi.Entities
{
    public class Order : BaseEntity
    {

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime DatePurchaced { get; set; }

        public DateTime DateDelivered { get; set; }

        public bool IsDelivered { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

        public int? CategoryId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        public int? ProductId { get; set; }

        [JsonIgnore]
        public User? User { get; set; } 

        public int? UserId { get; set; }

    }
}
