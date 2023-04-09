using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTechEcommerceApi.Data;

namespace EcommerceApi.Entities
{
    [Owned]
    public class RefreshToken : BaseEntity
    {
        public string? Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string? RevokedIp { get; set; }
        public string? ReplacedToken { get; set; }
        public string? ReasonRevoked { get; set; }
        public bool IsExpired  => DateTime.Now >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;

    }
}
