using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Account
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ParentAccountId { get; set; }
    public AccountType AccountType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime LastLocationUpdateTime { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ProfilePicture { get; set; }
    public string TaxNumber { get; set; }
    public decimal TaxRate { get; set; } = 20;
    public decimal CommissionRate { get; set; } = 10;
    public bool IsAccountCompleted { get; set; }
    public string InvoiceAddress { get; set; }
    
    public ICollection<AccountLocation> AccountLocations { get; set; }
    public ICollection<AccountFile> AccountFiles { get; set; }
    public ICollection<AccountVehicle> AccountVehicles { get; set; }
    
}
