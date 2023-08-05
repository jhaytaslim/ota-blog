using Application.DTOs.Utility;
using Application.Helpers;

namespace Application.Contracts.Utility;
public interface IEnumService
{
    SuccessResponse<EnumListDto> GetAllEnums();
}

