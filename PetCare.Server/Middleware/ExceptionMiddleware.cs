using Hangfire.Dashboard;
using PetCare.Shared.DTOs;
using PetCare.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace PetCare.Server.Middleware
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
    public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(httpContext, ex);
      }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      if (exception is BaseException baseEx)
      {
        context.Response.StatusCode = baseEx.StatusCode;
        var response = new BaseResponse()
        {
          Succeeded = false,
          Error = baseEx.ErrorMessage,
        };
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, serializeOptions));
      }
      else
      {
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
          StatusCode = context.Response.StatusCode,
          Message = exception.Message
        }));
      }
    }
  }
}
