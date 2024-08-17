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

public class CompanyVehicle
{
    [Key]
    public Guid Id { get; set; }
    public string Plate { get; set; }
    public Guid CompanyId { get; set; }
    
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }


    public ICollection<VehicleFile> VehicleFiles { get; set; }
    public ICollection<VehicleExtraService> VehicleExtraServices { get; set; }
    public ICollection<VehicleSegmentFilter> VehicleSegmentFilters { get; set; }
    public ICollection<VehicleTypeFilter> VehicleTypeFilters { get; set; }
    
    
}

public class VehicleBrand
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<VehicleModel> VehicleModels { get; set; }
}

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