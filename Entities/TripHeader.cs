using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class TripHeader
{
    [Key]
    public Guid Id { get; set; }
    
    public decimal TotalExtraServiceCost { get; set; }
    public decimal TotalTripCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Fee { get; set; }
    
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; }

    public ICollection<Trip> Trips { get; set; }
}