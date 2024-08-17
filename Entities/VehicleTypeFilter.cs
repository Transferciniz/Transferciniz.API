using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleTypeFilter
{
    [Key]
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }

    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; }
}