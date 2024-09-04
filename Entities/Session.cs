using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Session
{
    [Key]
    public Guid Id { get; set; }
    public Guid RelatedId { get; set; }
    public SessionType SessionType { get; set; }
    public DateTime LastActivity { get; set; }
}