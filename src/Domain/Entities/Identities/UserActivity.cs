using Domain.Common;
using Domain.Entities.Identities;

namespace Domain.Entities.Identities;
public class UserActivity : AuditableEntity
{
    public string EventType { get; set; }
    public string ObjectClass { get; set; }
    public Guid ObjectId { get; set; }
    public string Details { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
