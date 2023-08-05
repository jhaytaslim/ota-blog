

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
}

public record LoggedInUserDto
{
    public string StaffId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid OrganizationId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string OrganizationStatus { get; set; }
    public string Role { get; set; }
}