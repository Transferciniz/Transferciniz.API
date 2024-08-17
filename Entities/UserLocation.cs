using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Transferciniz.API.Entities;

public class UserLocation
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Geometry Location { get; set; }
    public DateTime UpdatedAt { get; set; }
}