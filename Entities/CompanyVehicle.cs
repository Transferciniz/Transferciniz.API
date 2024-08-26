using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

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