using System.ComponentModel.DataAnnotations;
using Transferciniz.API.DTOs;

namespace Transferciniz.API.Entities;

public class Trip
{
    [Key]
    public Guid Id { get; set; }

    public Guid TripHeaderId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid AccountVehicleId { get; set; }
    public AccountVehicle AccountVehicle { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Route { get; set; }

    public ICollection<WayPoint> WayPoints { get; set; }
    public TripStatus Status { get; set; }
    public decimal Cost { get; set; }
    public TripDirection TripDirection { get; set; }
    public TripBound Bounds { get; set; }

    public TripDto ToDto()
    {
        return new TripDto
        {
            Id = Id,
            AccountVehicleId = AccountVehicle.Id,
            StartDate = StartDate,
            Status = Status,
            Route = Route,
            TripDirection = TripDirection,
            VehicleName = $"{AccountVehicle.Vehicle.VehicleBrand.Name} {AccountVehicle.Vehicle.VehicleModel.Name}",
            VehiclePlate = AccountVehicle.Plate,
            Waypoints = WayPoints.Select(x => x.ToDto()).ToList()
        };
    }

    public TripDto ToDto(Account driver)
    {
        return new TripDto
        {
            Id = Id,
            AccountVehicleId = AccountVehicle.Id,
            StartDate = StartDate,
            Status = Status,
            VehicleName = $"{AccountVehicle.Vehicle.VehicleBrand.Name} {AccountVehicle.Vehicle.VehicleModel.Name}",
            VehiclePlate = AccountVehicle.Plate,
            VehiclePhoto = AccountVehicle.Vehicle.VehicleModel.Photo,
            Waypoints = WayPoints.Select(x => x.ToDto()).OrderBy(x => x.Ordering).ToList(),
            DriverName = $"{driver.Name} {driver.Surname}",
            DriverPhoto = driver.ProfilePicture
        };
    }

}

public enum TripDirection
{
    From = 1,
    To = 2
}


public class TripBound
{
    [Key]
    public Guid Id { get; set; }
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }
}



public class WayPoint
{
    [Key]
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime EstimatedTimeOfArrival { get; set; }
    public WaypointStatus Status { get; set; }
    public ICollection<WayPointUser> WayPointUsers { get; set; }

    public WayPointDto ToDto()
    {
        return new WayPointDto
        {
            Id = Id,
            Longitude = Longitude,
            Latitude = Latitude,
            Name = Name,
            Users = WayPointUsers.Select(x => x.ToDto()).ToList()
        };
    }
}

public class WayPointUser
{
    [Key]
    public Guid Id { get; set; }
    public Guid WayPointId { get; set; }

    public Guid? AccountId { get; set; }
    public Account? Account { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public bool WillCome { get; set; }
    public bool IsCame { get; set; }

    public WayPointUserDto ToDto()
    {
        return new WayPointUserDto
        {
            Id = Id,
            AccountId = AccountId,
            Name = Account?.Name ?? Name,
            Surname = Account?.Surname ?? Surname,
            ProfilePicture = Account?.ProfilePicture ?? "",
            IsCame = IsCame,
            WillCome = WillCome
        };
    }
    
}

public enum WaypointStatus
{
    Waiting = 0,
    OnRoad = 1,
    Near1Km = 2,
    Near500Mt = 3,
    Near200Mt = 4,
    OnWaypoint = 5,
    InProgress = 6,
    Finished = 7
}
