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
    
    public TripHeaderDto ToCustomerDto(Guid userId, List<Account> drivers)
    {
        var dto = new TripHeaderDto();
        var userTrip = Trips
            .FirstOrDefault(x => x.WayPoints
                .Any(y => y.WayPointUsers
                    .Any(w => w.AccountId == userId)));
        var userWaypoint = userTrip.WayPoints.First(x => x.WayPointUsers.Any(x => x.AccountId == userId));
        var driver = drivers.FirstOrDefault(x => x.Id == userTrip.AccountVehicle.DriverId);
        dto.Id = userTrip.Id;
        dto.Name = Name;
        dto.Status = userTrip.Status;
        dto.Plate = userTrip.AccountVehicle.Plate;
        dto.VehiclePhoto = userTrip.AccountVehicle.Vehicle.VehicleModel.Photo;
        dto.WillCome = userWaypoint.WayPointUsers.First(x => x.AccountId == userId).WillCome;
        dto.StartDate = userWaypoint.EstimatedTimeOfArrival;
        dto.WaypointUserId = userWaypoint.WayPointUsers.First(x => x.AccountId == userId).Id;
        dto.Route = userTrip.Route;
        dto.WaypointLatitude = userWaypoint.Latitude;
        dto.WaypointLongitude = userWaypoint.Longitude;
        dto.WaypointStatus = userWaypoint.Status;
        dto.DriverName = driver is not null ? $"{driver.Name} {driver.Surname}" : "";
        dto.DriverPhoto = driver is not null ? $"{driver.ProfilePicture}" : "";
        return dto;
    }
}