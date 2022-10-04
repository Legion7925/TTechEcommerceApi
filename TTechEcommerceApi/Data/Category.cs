using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommerceApi.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }

    }
}
