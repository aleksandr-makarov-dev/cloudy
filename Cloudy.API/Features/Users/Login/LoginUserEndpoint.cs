using System.Security.Claims;
using System.Text;
using Carter;
using Cloudy.API.Infrastructure.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Cloudy.API.Features.Users.Login;

public sealed class LoginUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/login", async (
                [FromBody] LoginUserRequest request,
                [FromServices] UserManager<IdentityUser<Guid>> userManager,
                [FromServices] IConfiguration configuration) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user is null)
                {
                    return Results.Unauthorized();
                }

                var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

                if (!isPasswordCorrect)
                {
                    return Results.Unauthorized();
                }

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
                var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims =
                [
                    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Email, user.Email!)
                ];

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("Jwt:Expiration")),
                    SigningCredentials = credentials,
                    Issuer = configuration["Jwt:Issuer"],
                    Audience = configuration["Jwt:Audience"],
                };

                var tokenHandler = new JsonWebTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Results.Ok(new { token });
            })
            .AddEndpointFilter<ValidationFilter<LoginUserRequest>>()
            .WithTags(Tags.Users);
    }
}