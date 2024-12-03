using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Commands.TripCommands;
using Transferciniz.API.Entities;

namespace Transferciniz.API
{
    public class TransportationContext : DbContext
    {
        public TransportationContext(DbContextOptions<TransportationContext> options): base(options)
        {
            
        }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountFile> AccountFiles { get; set; }
        public DbSet<AccountLocation> AccountLocations { get; set; }
        public DbSet<AccountVehicle> AccountVehicles { get; set; }

        public DbSet<AccountNotification> AccountNotifications { get; set; }
        
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBrand> VehicleBrands { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<VehicleFile> VehicleFiles { get; set; }
        public DbSet<VehicleExtraService> VehicleExtraServices { get; set; }
        public DbSet<VehicleSegmentFilter> VehicleSegmentFilters { get; set; }
        public DbSet<VehicleTypeFilter> VehicleTypeFilters { get; set; }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripHeader> TripHeaders { get; set; }
        public DbSet<TripExtraService> TripExtraServices { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExtraService> ExtraServices { get; set; }
        public DbSet<VehicleSegment> VehicleSegments { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<WayPoint> WayPoints { get; set; }
        public DbSet<WayPointUser> WayPointUsers { get; set; }
        public DbSet<TripHistory> TripHistories { get; set; }
        public DbSet<AccountVehicleProblem> AccountVehicleProblems { get; set; }
        public DbSet<AccountVehicleProblemHistory> AccountVehicleProblemHistories { get; set; }
    }
}