using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Transferciniz.API.Entities;

public class Trip
{
    [Key]
    public Guid Id { get; set; }

    public Guid TripHeaderId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid CompanyVehicleId { get; set; }

    public DateTime StartDate { get; set; }

    public List<Geometry> WayPoints { get; set; }
    
    public ICollection<TripExtraService> TripExtraServices { get; set; }

    public decimal TotalExtraServiceCost { get; set; }
    public decimal TotalTripCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Fee { get; set; }

}