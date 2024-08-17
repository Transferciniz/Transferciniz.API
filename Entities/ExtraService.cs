using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class ExtraService
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}