using Transferciniz.API.Entities;

namespace Transferciniz.API.DTOs;

public class TripDto
{
    public Guid Id { get; set; }
    public Guid AccountVehicleId { get; set; }
    public DateTime StartDate { get; set; }
    public string VehicleName { get; set; }
    public string VehiclePlate { get; set; }
    public List<WayPointDto> Waypoints { get; set; }
    public TripStatus Status { get; set; }
}