using Application.Contracts;
using Application.DTOs.Utility;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Utility;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/utility")]
public class UtilityController : ControllerBase
{
    private readonly IServiceManager _service;

    public UtilityController(IServiceManager service)
    {
        _service = service;
    }
    /// <summary>
    /// Endpoint to get all enums and or constants on the system.
    /// </summary>
    /// <returns>all Enum</returns>
    [HttpGet("enums")]
    [ProducesResponseType(typeof(SuccessResponse<EnumListDto>), 200)]
    public IActionResult GetEnum()
    {
        var response = _service.EnumService.GetAllEnums();
        return Ok(response);
    }
}

