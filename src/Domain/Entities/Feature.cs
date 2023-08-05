using Domain.Common;

namespace Domain.Entities;

public class Feature : AuditableEntity
{
    public string FeatureId { get; set; }

    public string Name { get; set; }

    public string Group { get; set; }

    public string Module { get; set; }

    public string Description { get; set; }

    public string Status { get; set; }
}