using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleBrand
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<VehicleModel> VehicleModels { get; set; }
}