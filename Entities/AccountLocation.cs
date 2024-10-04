using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Transferciniz.API.Entities;

public class AccountLocation
{
    [Key] 
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    public bool IsDefault { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime UpdatedAt { get; set; }
}