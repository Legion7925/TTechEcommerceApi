using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TTechEcommerceApi.Data
{
    public class BaseEntity
    {
        public int Id { get; protected set; }
    }
}
