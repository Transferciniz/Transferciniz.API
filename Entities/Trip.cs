using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Trip
{
    [Key]
    public Guid Id { get; set; }

    public Guid TripHeaderId { get; set; }
    public Guid DriverId { get; set; }
    public Guid CompanyVehicleId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string RouteJson { get; set; }

    public int PersonCount { get; set; }
    public int ChildCount { get; set; }
    public int HandicapedPersonCount { get; set; }

    public ICollection<TripExtraService> TripExtraServices { get; set; }

    public decimal TotalExtraServiceCost { get; set; }
    public decimal TotalTripCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Fee { get; set; }

}