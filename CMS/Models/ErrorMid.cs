using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;


public class ErrorMid
{
    private readonly RequestDelegate next;

    public ErrorMid(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
        try
        {
            await next(context);
        }
        catch (HttpStatusCodeException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception exceptionObj)
        {
            await HandleExceptionAsync(context, exceptionObj);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, HttpStatusCodeException exception)
    {
        string result = null;
        context.Response.ContentType = "application/json";
        if (exception is HttpStatusCodeException)
        {
            result = new ErrorDetails() { Message = exception.InnerException.Message, StatusCode = (int)exception.StatusCode }.ToString();
            context.Response.StatusCode = (int)exception.StatusCode;
        }
        else
        {
            result = new ErrorDetails() { Message = "Runtime Error", StatusCode = (int)HttpStatusCode.BadRequest }.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        return context.Response.WriteAsync(result);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string result = new ErrorDetails() { Message = exception?.InnerException?.Message + "\n" + "\n" + exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError }.ToString();
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteAsync(result);
    }
}
public class HttpStatusCodeException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string ContentType { get; set; } = @"text/plain";

    public HttpStatusCodeException(HttpStatusCode statusCode)
    {
        this.StatusCode = statusCode;
    }

    public HttpStatusCodeException(HttpStatusCode statusCode, string message) : base(message)
    {
        this.StatusCode = statusCode;
    }

    public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

    public HttpStatusCodeException(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
    {
        this.ContentType = @"application/json";
    }

}

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}


