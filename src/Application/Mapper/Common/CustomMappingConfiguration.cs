using Domain.Extensions;
using Domain.Enums.Common;
using Domain.Enums;
using AutoMapper;

namespace Application.Mapper.Common;

public class CustomMappingConfiguration : Profile
{
    public CustomMappingConfiguration()
    {
        CreateMap<EStatus, string>().ConvertUsing(src => src.GetDescription());
    }
}

