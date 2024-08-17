using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Transferciniz.API.Entities;

public class VehicleLocation
{
    [Key]
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    
    public Geometry Location { get; set; }
    public DateTime UpdatedAt { get; set; }
}