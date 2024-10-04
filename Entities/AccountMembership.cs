using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class AccountMembership
{
    [Key]
    public int Id { get; set; }

    public Guid AccountId { get; set; }
    public Guid MemberId { get; set; }
    public MembershipType MembershipType { get; set; }
}