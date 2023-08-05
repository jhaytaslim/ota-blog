using Domain.Helpers;
using Microsoft.AspNetCore.Http;

namespace Application.Helpers
{
    public interface IWebHelper
    {
        UserHelperDTO User();
    }

    public class WebHelper : IWebHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        public static UserHelperDTO UserHelper
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypeHelper.UserId).FirstOrDefault()?.Value ?? "";
                var organizationId = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.OrganizationId).FirstOrDefault()?.Value ?? null;
                var fullName = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.FullName).FirstOrDefault()?.Value ?? "";
                var email = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.Email).FirstOrDefault()?.Value ?? "";
                var userName = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.UserName).FirstOrDefault()?.Value ?? "";
                var employeeId = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.EmployeeId).FirstOrDefault()?.Value ?? null;
                var role = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.RolesStr).FirstOrDefault()?.Value ?? "";

                var result = new UserHelperDTO
                {
                    OrganizationId = string.IsNullOrEmpty(organizationId) ? Guid.Empty : Guid.Parse(organizationId),
                    UserId = userId,
                    UserName = userName,
                    FullName = fullName,
                    Email = email,
                    EmployeeId = string.IsNullOrEmpty(employeeId) ? Guid.Empty : Guid.Parse(employeeId),
                    Role = role,
                };
                return result;
            }
        }

        public UserHelperDTO User()
        {
            return UserHelper;
        }
    }

    public class UserHelperDTO
    {
        public Guid OrganizationId { get; set; }
        public string UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
