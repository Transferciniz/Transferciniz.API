using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using Transferciniz.API.DTOs;

namespace Transferciniz.API.Entities;

public class AccountVehicle
{
    [Key]
    public Guid Id { get; set; }
    public string Plate { get; set; }
    public Guid AccountId { get; set; }
    public Guid? DriverId { get; set; }
    
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public VehicleStatus Status { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<VehicleFile> VehicleFiles { get; set; }
    public ICollection<VehicleExtraService> VehicleExtraServices { get; set; }
    public ICollection<VehicleSegmentFilter> VehicleSegmentFilters { get; set; }
    public ICollection<VehicleTypeFilter> VehicleTypeFilters { get; set; }
    public ICollection<AccountVehicleProblem> AccountVehicleProblems { get; set; }

    public AccountVehicleDto ToDto() =>
        new()
        {
            Id = Id,
            Latitude = Latitude,
            Longitude = Longitude,
            Name = $"{Vehicle.VehicleBrand.Name} {Vehicle.VehicleModel.Name}",
            Plate = Plate,
            Status = Status,
            Photo = Vehicle.VehicleModel.Photo
        };
}

public enum VehicleStatus
{
    Offline = 0,
    Online = 1,
    Busy = 2,
    InMaintenance = 3,
    InAccident = 4,
    HasProblem = 5
}

public class AccountVehicleProblem
{
    public Guid Id { get; set; }
    public Guid AccountVehicleId { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public string Message { get; set; }
    public AccountVehicleProblemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public ICollection<AccountVehicleProblemHistory> AccountVehicleProblemHistories { get; set; }
}

public class AccountVehicleProblemHistory
{
    public Guid Id { get; set; }
    public Guid AccountVehicleProblemId { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public AccountVehicleProblemStatus FromStatus { get; set; }
    public AccountVehicleProblemStatus ToStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum AccountVehicleProblemStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2
}
