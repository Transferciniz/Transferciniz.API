namespace Transferciniz.API.DTOs;

public class WayPointUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid? AccountId { get; set; }
    public string? ProfilePicture { get; set; }
}