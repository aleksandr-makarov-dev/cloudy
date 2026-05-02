using Carter;
using Cloudy.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloudy.API.Endpoints.Items.Get;

public sealed class GetItemsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/items", async (
                [FromServices] ApplicationDbContext dbContext,
                CancellationToken cancellationToken
            ) =>
            {
                var items = await dbContext.Items
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new ItemResponse(
                        x.Id,
                        $"{x.Name}.{x.Extension}",
                        x.ContentType,
                        x.Size,
                        x.CreatedAt
                    ))
                    .ToListAsync(cancellationToken);

                return Results.Ok(items);
            })
            .WithTags(Tags.Items);
    }
}