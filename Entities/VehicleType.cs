using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleType
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
}