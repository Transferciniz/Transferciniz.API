using System.ComponentModel.DataAnnotations;

namespace Transferciniz.API.Entities;

public class TripHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid TripId { get; set; }
    public string Message { get; set; }
    public DateTime DateTime { get; set; }
}