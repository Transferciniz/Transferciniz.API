using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleSegmentFilter
{
    [Key]
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }

    public Guid VehicleSegmentId { get; set; }
    public VehicleSegment VehicleSegment { get; set; }
}