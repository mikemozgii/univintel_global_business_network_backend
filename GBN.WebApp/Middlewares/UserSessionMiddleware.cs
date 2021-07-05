using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.Services;

namespace UnivIntel.GBN.WebApp.Middlewares
{
    public class UserSessionMiddleware
    {
        private readonly RequestDelegate m_RequestDelegate;

        public UserSessionMiddleware(RequestDelegate requestDelegate)
        {
            m_RequestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.HasValue ? context.Request.Path.Value.ToLowerInvariant() : "";

            var pathsApi = new List<string> {
                "/api/1/authentification/signup",
                "/api/1/authentification/signin",
                "/api/1/authentification/trysignin",
                "/api/1/authentification/verify",
                "/api/1/authentification/signout",
                "/api/1/authentification/forgotpassword",
                "/api/1/localization/all"
            };

            var isSessionPath = path.StartsWith("/api") && !pathsApi.Contains(path);

            if (!isSessionPath)
            {
                await m_RequestDelegate(context);
                return;
            }

            var token = await GetUserSessionFromContext(context);

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentLength = 0;
                return;
            }

            context.Items["PageSession"] = token;

            await m_RequestDelegate(context);
        }

        private async Task<string> GetUserSessionFromContext(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey("GBNToken")) return null;

            var token = context.Request.Cookies["GBNToken"];
            var userSession = await AuthentificationService.GetOrRaiseExpireUserSession(Guid.Parse(token));
            if (userSession != null) return token;

            return null;
        }

    }
}
