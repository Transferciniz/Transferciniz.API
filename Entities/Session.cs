using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Session
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime LastActivity { get; set; }
}