namespace Cloudy.API.Infrastructure.Authentication;

public interface IUserContext
{
    public Guid UserId { get; }
}