using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleActivity
{
    [Key]
    public Guid Id { get; set; }
    public Guid AccountVehicleId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}