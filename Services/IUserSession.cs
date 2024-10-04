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
    public AccountType AccountType { get; set; }
}

public class UserSession : IUserSession
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public AccountType AccountType { get; set; }


    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null && context.User.Identity.IsAuthenticated)
        {
            Enum.TryParse<AccountType>(context.User.Claims.First(x => x.Type == "accountType").Value, out var userType);
            Name = context.User.Claims.First(x => x.Type == "name").Value;
           // TODO: fix this Email = context.User.Claims.First(x => x.Type == "email").Value;
            Surname = context.User.Claims.First(x => x.Type == "surname").Value;
            AccountType = userType;
            Id = Guid.Parse(context.User.Claims.First(x => x.Type == "id").Value);
            SessionId = Guid.Parse(context.User.Claims.First(x => x.Type == "sessionId").Value);
        }
    }
}