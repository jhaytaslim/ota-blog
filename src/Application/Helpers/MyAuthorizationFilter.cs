﻿using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Application.Helpers;
public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    IConfiguration configuration;

    public HangFireAuthorizationFilter(IConfiguration config)
    {
        configuration = config;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        var header = httpContext.Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(header))
        {
            SetChallengeResponse(httpContext);
            return false;

        }

        var authvalues = AuthenticationHeaderValue.Parse(header);

        if (!"Basic".Equals(authvalues.Scheme, StringComparison.InvariantCultureIgnoreCase))
        {
            SetChallengeResponse(httpContext);
            return false;
        }

        var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authvalues.Parameter));
        var parts = parameter.Split(':');

        var username = parts[0];
        var password = parts[1];


        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetChallengeResponse(httpContext);
            return false;
        }

        var hgSettings = configuration.GetSection("HangFireSettings");
        var hgPassword = hgSettings.GetSection("Password").Value;
        var hgUsername = hgSettings.GetSection("Username").Value;
        if (username == hgPassword && password == hgUsername)
        {
            return true;
        }

        SetChallengeResponse(httpContext);
        return false;
    }

    private void SetChallengeResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
        httpContext.Response.WriteAsync("Authentication is required");
    }
}

