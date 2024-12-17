using System.ComponentModel.DataAnnotations;
using Transferciniz.API.DTOs;

namespace Transferciniz.API.Entities;

public class TripHeader
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid AccountId { get; set; }

    public decimal Cost { get; set; }
    
    public TripStatus Status { get; set; }

    public ICollection<Trip> Trips { get; set; }

    public TripHeaderDto ToDto() => new()
        {
            Id = Id,
            Name = Name,
            Status = Trips.Select(x => x.Status).All(x => x == TripStatus.Live) ? TripStatus.Live : TripStatus.Approved,
        };
    
    public TripHeaderDto ToCustomerDto(Guid userId) => new()
    {
        Id = Id,
        Name = Name,
        Status = Trips
            .FirstOrDefault(x => x.WayPoints
                .Any(y => y.WayPointUsers
                    .Any(w => w.AccountId == userId)))
            !.Status ,
        Plate = Trips.First().AccountVehicle.Plate,
        VehiclePhoto = Trips.First().AccountVehicle.Vehicle.VehicleModel.Photo,
        WillCome = Trips.First().WayPoints.SelectMany(x => x.WayPointUsers).First(x => x.AccountId == userId).WillCome,
        WaypointUserId = Trips.First().WayPoints.SelectMany(x => x.WayPointUsers).First(x => x.AccountId == userId).Id,
    };
}