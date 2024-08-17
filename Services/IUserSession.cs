using System.Security.Claims;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Services;

public interface IUserSession
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid? CompanyId { get; set; }
    public UserType UserType { get; set; }
}

public class UserSession : IUserSession
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid? CompanyId { get; set; }
    public UserType UserType { get; set; }


    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null && context.User.Identity.IsAuthenticated)
        {
            Enum.TryParse<UserType>(context.User.Claims.First(x => x.Type == "userType").Value, out var userType);
            Name = context.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            Email = context.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            Surname = context.User.Claims.First(x => x.Type == "surname").Value;
            UserType = userType;
            Id = Guid.Parse(context.User.Claims.First(x => x.Type == "id").Value);
            SessionId = Guid.Parse(context.User.Claims.First(x => x.Type == "sessionId").Value);
            CompanyId = Guid.Parse(context.User.Claims.First(x => x.Type == "companyId")?.Value ?? Guid.Empty.ToString());
        }
    }
}