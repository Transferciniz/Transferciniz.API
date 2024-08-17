namespace Transferciniz.API.Entities;

public class VehicleExtraService
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    
    public Guid ExtraServiceId { get; set; }
    public ExtraService ExtraService { get; set; }
}