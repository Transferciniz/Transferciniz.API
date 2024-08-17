namespace Transferciniz.API.Entities;

public class TripExtraService
{
    public Guid Id { get; set; }
    
    public Guid TripId { get; set; }
    public Guid ExtraServiceId { get; set; }
}