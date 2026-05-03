using Carter;
using Cloudy.API.Infrastructure.Authentication;
using Cloudy.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloudy.API.Features.Items.Get;

public sealed class GetItemsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/items", async (
                [FromQuery] Guid? parentId,
                [FromServices] ApplicationDbContext dbContext,
                [FromServices] IUserContext userContext,
                CancellationToken cancellationToken
            ) =>
            {
                var items = await dbContext.Items
                    .AsNoTracking()
                    .Where(x => x.ParentId == parentId && x.UserId == userContext.UserId)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new ItemResponse(
                        x.Id,
                        x.IsFolder ? x.Name : $"{x.Name}.{x.Extension}",
                        x.ContentType,
                        x.Size,
                        x.CreatedAt
                    ))
                    .ToListAsync(cancellationToken);

                return Results.Ok(items);
            })
            .RequireAuthorization()
            .WithTags(Tags.Items);
    }
}