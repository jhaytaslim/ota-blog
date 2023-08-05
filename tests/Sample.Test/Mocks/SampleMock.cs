using Application.DTOs;
using Application.Helpers;
using Domain.Enums;
using Bogus;

namespace SampleService.Test.Mocks
{
    public record SampleMock
    {
        public static Faker<SampleDto> ResponseDTOMock = new Faker<SampleDto>();

        public static SuccessResponse<SampleDto> SuccessResponseDTOMock = new SuccessResponse<SampleDto>
        {
            Message = "Success",
            Data = ResponseDTOMock
        };
    }

}