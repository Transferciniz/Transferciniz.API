using Transferciniz.API.Entities;

namespace Transferciniz.API.DTOs;

public class AccountVehicleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Plate { get; set; }
    public string Photo { get; set; }
    public VehicleStatus Status { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}