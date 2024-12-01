using Microsoft.AspNetCore.Authorization;

namespace Transferciniz.API.Helpers;

public class SessionRequirement: IAuthorizationRequirement { }

public class SessionRequirementHandler : AuthorizationHandler<SessionRequirement>
{
    private readonly TransportationContext _dbContext;

    public SessionRequirementHandler(TransportationContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SessionRequirement requirement)
    {
        var sessionId = context.User.Claims.FirstOrDefault(c => c.Type == "sessionId")?.Value;
        if (string.IsNullOrEmpty(sessionId))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        try
        {
            if (_dbContext.Sessions.Any(s => s.Id == Guid.Parse(sessionId)))
            {
                context.Succeed(requirement);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Fail(new AuthorizationFailureReason(this, "Hata var"));
            throw;
        }

     

        return Task.CompletedTask;
    }
}