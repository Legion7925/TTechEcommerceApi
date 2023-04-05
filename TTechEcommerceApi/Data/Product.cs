using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTechEcommerceApi.Data;

namespace EcommerceApi.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }

        [Range(1, int.MaxValue , ErrorMessage = "the price can not be 0")]
        public double Price { get; set; }

        public string? Color { get; set; }

        public string? ImagePath { get; set; }

        public bool IsAvailable { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }

        public int CategoryId { get; set; }

    }
}
