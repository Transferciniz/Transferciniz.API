using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class AccountNotification
{
    [Key]
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public bool IsViewed { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; }
}