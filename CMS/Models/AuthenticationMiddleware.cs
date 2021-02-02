using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
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
        if (SessionRequest._User == null && !httpContext.Request.Path.Value.ToLower().Contains("/login"))
            httpContext.Response.Redirect("/login/login1");
        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationMiddleware>();
    }
}
