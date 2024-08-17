using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class Company
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string TaxNumber { get; set; }
    public CompanyType CompanyType { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; }
    public ICollection<CompanyFile> CompanyFiles { get; set; }
}