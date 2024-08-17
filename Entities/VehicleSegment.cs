using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class VehicleSegment
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
}