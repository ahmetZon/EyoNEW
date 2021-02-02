using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


public class AuthenticationMiddleware
{
    RequestDelegate _next;
    IHttpContextAccessor httpContextAccessor;

    public AuthenticationMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        this._next = next;
        this.httpContextAccessor = httpContextAccessor;
    }

    public Task Invoke(HttpContext httpContext)
    {
      //  if (httpContextAccessor.HttpContext.Session.GetString("_user") == null && !httpContext.Request.Path.Value.ToLower().Contains("/login"))
      //  {
      //      httpContext.Response.Redirect("/Base/Index");
      //  }
        return _next(httpContext);
    }
}

public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationMiddleware>();
    }
}
