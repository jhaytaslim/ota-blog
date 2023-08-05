using Domain.Common;
using Domain.Entities.Identities;

namespace Domain.Entities
{
    public class Token : AuditableEntity
    {
        public string Value { get; set; }
        public Guid UserId { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(3);
        public User User { get; set; }
    }
}
