using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Company
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string TaxNumber { get; set; }
    public string ProfilePicture { get; set; }
    public CompanyType CompanyType { get; set; }

    public decimal TaxRate { get; set; } = 20;
    public decimal CommissionRate { get; set; } = 10;

    public ICollection<CompanyVehicle> CompanyVehicles { get; set; }
    public ICollection<CompanyFile> CompanyFiles { get; set; }
}