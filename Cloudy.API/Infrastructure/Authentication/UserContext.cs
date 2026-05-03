using System.Security.Claims;

namespace Cloudy.API.Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor context) : IUserContext
{
    public Guid UserId => context.HttpContext?.User.GetUserId() ??
                          throw new InvalidOperationException("User not authenticated.");
}