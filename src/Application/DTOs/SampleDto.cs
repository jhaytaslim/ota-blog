
namespace Application.DTOs;

public record SampleDto : AuditableDto
{
    public string Name { get; set; }
    public int Age { get; set; }
}