namespace Transferciniz.API.DTOs;

public class WayPointDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Ordering { get; set; }
    public List<WayPointUserDto> Users { get; set; }
}