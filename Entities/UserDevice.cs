using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class UserDevice
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string ApplicationVersion { get; set; }
    public string DeviceInfo { get; set; }
    
}