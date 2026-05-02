using Carter;
using Cloudy.API.Data;
using Cloudy.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;

namespace Cloudy.API.Endpoints.Items.Upload;

public sealed class UploadItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/items/upload", async (
                [FromForm] UploadItemRequest request,
                [FromServices] ApplicationDbContext dbContext,
                [FromServices] IMinioClient minioClient,
                [FromServices] ILogger<UploadItemEndpoint> logger,
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

                try
                {
                    var id = Guid.NewGuid();
                    var name = Path.GetFileNameWithoutExtension(request.File.FileName);
                    var extension = Path.GetExtension(request.File.FileName).Replace(".", string.Empty);
                    var contentType = request.File.ContentType;
                    var size = request.File.Length;
                    var key = $"{id}.{extension}";

                    // Upload file to s3 bucket
                    await using (var fileStream = request.File.OpenReadStream())
                    {
                        var putObjectArgs = new PutObjectArgs()
                            .WithBucket(Constants.BucketName)
                            .WithObject(key)
                            .WithStreamData(fileStream)
                            .WithContentType(contentType)
                            .WithObjectSize(size);

                        await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
                    }

                    // Save item metadata to database
                    var item = new Item
                    {
                        Id = id,
                        Name = name,
                        Extension = extension,
                        ContentType = contentType,
                        Size = size,
                        IsFolder = false,
                        CreatedAt = DateTime.UtcNow,
                        ParentId = request.ParentId
                    };

                    dbContext.Items.Add(item);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    logger.LogInformation("Item '{}' uploaded successfully.", item.Id);

                    return Results.Ok(new { id = item.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while uploading item.");

                    return Results.BadRequest();
                }
            })
            .WithTags(Tags.Items)
            .DisableAntiforgery();
    }
}