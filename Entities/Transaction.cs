using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Transaction
{
    [Key]
    public Guid Id { get; set; }
    public string ProviderTransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid TripId { get; set; }
    public decimal Amount { get; set; }
}