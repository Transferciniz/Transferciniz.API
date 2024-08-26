using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries.Prepared;

namespace Transferciniz.API.Entities;

public class Vehicle
{
    [Key]
    public Guid Id { get; set; }

    public Guid VehicleBrandId { get; set; }
    public VehicleBrand VehicleBrand { get; set; }
    
    public Guid VehicleModelId { get; set; }
    public VehicleModel VehicleModel { get; set; }

    public decimal BasePrice { get; set; }
    
}