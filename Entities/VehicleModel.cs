using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleModel
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int ExtraCapacity { get; set; }
    public int TotalCapacity => Capacity + ExtraCapacity;
    
    public Guid VehicleBrandId { get; set; }

}