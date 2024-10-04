using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Transferciniz.API.Entities;

public class AccountVehicle
{
    [Key]
    public Guid Id { get; set; }
    public string Plate { get; set; }
    public Guid AccountId { get; set; }
    
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public Geometry Location { get; set; }

    public ICollection<VehicleFile> VehicleFiles { get; set; }
    public ICollection<VehicleExtraService> VehicleExtraServices { get; set; }
    public ICollection<VehicleSegmentFilter> VehicleSegmentFilters { get; set; }
    public ICollection<VehicleTypeFilter> VehicleTypeFilters { get; set; }
    
    
}