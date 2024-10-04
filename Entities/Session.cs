using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Session
{
    [Key]
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public DateTime LastActivity { get; set; }
}