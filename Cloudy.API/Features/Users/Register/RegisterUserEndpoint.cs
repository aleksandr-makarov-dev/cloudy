using Carter;
using Cloudy.API.Infrastructure.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cloudy.API.Features.Users.Register;

public sealed class RegisterUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/register", async (
                [FromBody] RegisterUserRequest request,
                [FromServices] UserManager<IdentityUser<Guid>> userManager) =>
            {
                var user = new IdentityUser<Guid>
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    EmailConfirmed = true
                };

                var identityResult = await userManager.CreateAsync(user, request.Password);

                if (!identityResult.Succeeded)
                {
                    return Results.BadRequest(identityResult.Errors);
                }

                return Results.Ok(new { message = "User created successfully." });
            })
            .AddEndpointFilter<ValidationFilter<RegisterUserRequest>>()
            .WithTags(Tags.Users);
    }
}