namespace Cloudy.API.Entities;

public sealed class Item
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Extension { get; set; } = string.Empty;
    
    public long Size { get; set; }
    
    public string ContentType { get; set; } =  string.Empty;
    
    public DateTime CreatedAt { get; set; }
}