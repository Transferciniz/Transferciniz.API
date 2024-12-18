using Transferciniz.API.Entities;

namespace Transferciniz.API.DTOs;

public class TripHeaderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public TripStatus Status { get; set; }
    public string? Plate { get; set; }
    public string? VehiclePhoto { get; set; }
    public bool? WillCome { get; set; }
    public Guid? WaypointUserId { get; set; }
    public string Route { get; set; }
    public double WaypointLatitude { get; set; }
    public double WaypointLongitude { get; set; }
    public WaypointStatus WaypointStatus { get; set; }
    public string? DriverName { get; set; }
    public string? DriverPhoto { get; set; }
}