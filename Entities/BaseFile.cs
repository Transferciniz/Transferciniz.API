using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class BaseFile
{
    [Key]
    public Guid Id { get; set; }
    public byte[] File { get; set; }
    public FileCategory FileCategory { get; set; }
}