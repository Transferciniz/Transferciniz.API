using Transferciniz.API.Entities;

namespace Transferciniz.API.DTOs;

public class TripHeaderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public TripStatus Status { get; set; }
    public string Plate { get; set; }
    public string VehiclePhoto { get; set; }
}