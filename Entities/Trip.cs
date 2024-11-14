using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;
using Skybrud.Essentials.Collections.Extensions;
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

    public ICollection<WayPoint> WayPoints { get; set; }
    public TripStatus Status { get; set; }
    
    public ICollection<TripExtraService> TripExtraServices { get; set; }

    public decimal TotalExtraServiceCost { get; set; }
    public decimal TotalTripCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Fee { get; set; }

    public TripDto ToDto()
    {
        return new TripDto
        {
            Id = Id,
            StartDate = StartDate,
            Status = Status,
            VehicleName = $"{AccountVehicle.Vehicle.VehicleBrand.Name} {AccountVehicle.Vehicle.VehicleModel.Name}",
            VehiclePlate = AccountVehicle.Plate,
            Waypoints = WayPoints.Select(x => x.ToDto()).OrderBy(x => x.Ordering).ToList()
        };
    }

}



public class WayPoint
{
    [Key]
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Name { get; set; }
    public int Ordering { get; set; }
    public bool IsCompleted { get; set; }
    public ICollection<WayPointUser> WayPointUsers { get; set; }

    public WayPointDto ToDto()
    {
        return new WayPointDto
        {
            Id = Id,
            Longitude = Longitude,
            Latitude = Latitude,
            Name = Name,
            Ordering = Ordering,
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
