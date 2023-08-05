using Domain.Common;
using Domain.Enums.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identities;

public class User : IdentityUser<Guid>, IAuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public bool Verified { get; set; } = false;
    public bool IsActive { get; set; } = false;
    public string Status { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedById { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}