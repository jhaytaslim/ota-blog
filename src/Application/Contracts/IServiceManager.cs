using Application.Contracts;
using Application.Contracts.Utility;

namespace Application.Contracts;

public interface IServiceManager
{
    IEnumService EnumService { get; }
}