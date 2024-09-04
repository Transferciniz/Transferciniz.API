using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ProfilePicture { get; set; }
    public UserType UserType { get; set; }

    public ICollection<UserFile> UserFiles { get; set; }

    
}