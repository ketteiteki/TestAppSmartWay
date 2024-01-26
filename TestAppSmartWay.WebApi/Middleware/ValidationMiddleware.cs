using FluentValidation;
using TestAppSmartWay.Domain.Responses.Errors;

namespace TestAppSmartWay.WebApi.Middleware;

public class ValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next.Invoke(httpContext);
        }
        catch (ValidationException e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsJsonAsync(new Error(string.Join("; ", e.Errors)));
        }
    }
}