using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    public VehicleStatus Status { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<VehicleFile> VehicleFiles { get; set; }
    public ICollection<VehicleExtraService> VehicleExtraServices { get; set; }
    public ICollection<VehicleSegmentFilter> VehicleSegmentFilters { get; set; }
    public ICollection<VehicleTypeFilter> VehicleTypeFilters { get; set; }
}

public enum VehicleStatus
{
    Offline = 0,
    Online = 1,
    Busy = 2,
    InMaintenance = 3
}
