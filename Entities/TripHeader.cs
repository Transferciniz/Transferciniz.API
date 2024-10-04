using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class TripHeader
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid AccountId { get; set; }
    
    public decimal TotalExtraServiceCost { get; set; }
    public decimal TotalTripCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Fee { get; set; }

    public DateTime StartDate { get; set; }
    public TripStatus Status { get; set; }

    public ICollection<Trip> Trips { get; set; }
}