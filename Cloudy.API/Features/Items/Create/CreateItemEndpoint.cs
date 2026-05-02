using Carter;
using Cloudy.API.Domain;
using Cloudy.API.Infrastructure.Data;
using Cloudy.API.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloudy.API.Features.Items.Create;

public sealed class CreateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/items", async (
                [FromBody] CreateItemRequest request,
                [FromServices] ApplicationDbContext dbContext,
                CancellationToken cancellationToken
            ) =>
            {
                // If parentId is provided than check if it is a folder and exists in database.
                if (request.ParentId.HasValue)
                {
                    var exists = await dbContext.Items.AnyAsync(x => x.IsFolder && x.Id == request.ParentId.Value,
                        cancellationToken);

                    if (!exists)
                    {
                        return Results.BadRequest("Parent folder does not exist.");
                    }
                }

                var id = Guid.NewGuid();

                var item = new Item
                {
                    Id = id,
                    Name = request.Name,
                    IsFolder = true,
                    CreatedAt = DateTime.UtcNow,
                    ParentId = request.ParentId
                };

                dbContext.Items.Add(item);

                await dbContext.SaveChangesAsync(cancellationToken);

                return Results.Ok(new { id = item.Id });
            })
            .AddEndpointFilter<ValidationFilter<CreateItemRequest>>()
            .WithTags(Tags.Items);
    }
}