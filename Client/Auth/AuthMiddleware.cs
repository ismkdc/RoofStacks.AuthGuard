using System.Net;

namespace Client.Auth;

public class AuthMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ForbiddenException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            Console.WriteLine("Forbidden request attempted");
        }
    }
}