using System.ComponentModel;

namespace Domain.Enums.Common;

public enum EStatus
{
    [Description("Active")]
    Active,
    [Description("Pending")]
    Pending,
    [Description("Disable")]
    Disable,
}