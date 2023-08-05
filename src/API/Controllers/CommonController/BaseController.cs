using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace API.Controllers.CommonController;

[ApiController]
public class BaseController : ControllerBase
{
    protected LoggedInUserDto LoggedInUser => new()
    {
        StaffId = WebHelper.UserHelper.UserId,
        FullName = WebHelper.UserHelper.FullName,
        Email = WebHelper.UserHelper.Email,
        OrganizationId = WebHelper.UserHelper.OrganizationId,
        EmployeeId = WebHelper.UserHelper.EmployeeId,
        Role = WebHelper.UserHelper.Role,
    };
}