using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Transferciniz.API.Entities;

namespace Transferciniz.API
{
    public class TransportationContext : DbContext
    {
        public TransportationContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMembership> UserMemberships { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyFile> CompanyFiles { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBrand> VehicleBrands { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<CompanyVehicle> CompanyVehicles { get; set; }
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
    }
}