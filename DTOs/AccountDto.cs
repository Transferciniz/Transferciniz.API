using Transferciniz.API.Entities;

namespace Transferciniz.API.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public AccountType AccountType { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string ProfilePicture { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}