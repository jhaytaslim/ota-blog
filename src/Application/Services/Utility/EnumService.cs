using Application.Contracts.Utility;
using Application.DTOs.Utility;
using Domain.Enums;
using Domain.Enums.Common;
using Domain.Enums;
using Domain.Extensions;
using Application.Helpers;

namespace Application.Services.Utility;
public class EnumService : IEnumService
{

    public SuccessResponse<EnumListDto> GetAllEnums()
    {
        var statusEnumList = EnumExtension.GetEnumResults<EStatus>();

        var enumList = new EnumListDto
        {
            StatusEnumList = statusEnumList,
        };

        return new SuccessResponse<EnumListDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = enumList
        };
    }
}

