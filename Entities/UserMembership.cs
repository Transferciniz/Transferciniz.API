using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class UserMembership
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public MembershipType MembershipType { get; set; }
}