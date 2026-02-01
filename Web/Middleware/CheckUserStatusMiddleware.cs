using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.ErrorModels;
using System;
using System.Security.Claims;

namespace Web.Middleware
{
    public class CheckUserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckUserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<UserApp> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true) // Check if the user is authenticated Mean Logged in Mean Has a valid token
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId); // Filter Add In DBContext Make EFCore Return Null If User.IsDeleted = True

                    if (user == null || user.IsDeleted) // Check if user is deleted or not found
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Your account has been deactivated."
                        });
                        return; 
                    }
                }
            }
            await _next(context);
        }
    }
}
