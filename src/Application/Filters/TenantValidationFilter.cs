using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Application.Helpers;
using Domain.Entities.Identities;
using Domain.Enums.Common;
using Microsoft.AspNetCore.Http;
using Infrastructure.Contracts;
using Application.Contracts;

namespace Application.Filters
{
    public class TenantValidationFilter : IActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly string _organizationIdText;
        public TenantValidationFilter(IRepositoryManager repository, string organizationIdText = "organizationId")
        {
            _repository = repository;
            _organizationIdText = organizationIdText;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // retrieve value from claims
            var controller = context.Controller as ControllerBase;

            if (!Convert.ToBoolean(controller?.User?.Identity?.IsAuthenticated))
                return;

            return;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private void SetObjectResult(ActionExecutingContext context, string message, int code = 403, object error = null)
        {
            context.Result = new ObjectResult(new
            {
                Success = false,
                Message = message,
                Error = error
            });
            context.HttpContext.Response.StatusCode = code;
            return;

        }

    }
}