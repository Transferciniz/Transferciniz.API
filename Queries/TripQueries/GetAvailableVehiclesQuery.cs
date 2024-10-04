using MediatR;
using Microsoft.EntityFrameworkCore;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Queries.TripQueries;

public class GetAvailableVehiclesQuery: IRequest<GetAvailableVehiclesQueryResponse>
{
    public int TotalPerson { get; set; }
    public List<Guid> ExtraServices { get; set; }
    public List<Guid> Segments { get; set; }
    public List<Guid> Types { get; set; }
    public decimal TotalLengthOfRoad { get; set; }
}

public class GetAvailableVehiclesQueryResponse: List<CombinationPricePair>
{
  
}

public class VehicleUsagePair
{
    public Vehicle Vehicle { get; set; }
    public int Usage { get; set; }
}

public class CombinationPricePair
{
    public List<VehicleUsagePair> Vehicles { get; set; }
    public decimal TotalPrice { get; set; }
}

class CombinationResult
{
    public Dictionary<Vehicle, int> VehicleUsage { get; set; }
    public int TotalPeople { get; set; }

    public CombinationResult(Dictionary<Vehicle, int> vehicleUsage, int totalPeople)
    {
        VehicleUsage = vehicleUsage;
        TotalPeople = totalPeople;
    }
}

public class GetAvailableVehiclesQueryHandler : IRequestHandler<GetAvailableVehiclesQuery, GetAvailableVehiclesQueryResponse>
{
    private readonly TransportationContext _context;

    public GetAvailableVehiclesQueryHandler(TransportationContext context)
    {
        _context = context;
    }

    public async Task<GetAvailableVehiclesQueryResponse> Handle(GetAvailableVehiclesQuery request, CancellationToken cancellationToken)
    {
        var extraServicesIncludedVehicles = await _context.VehicleExtraServices
            .Where(x => request.ExtraServices.Contains(x.ExtraServiceId))
            .Select(x => x.VehicleId)
            .ToListAsync(cancellationToken: cancellationToken);

        var segmentFilteredVehicles = await _context.VehicleSegmentFilters
            .Where(x => request.Segments.Contains(x.VehicleSegmentId))
            .Select(x => x.VehicleId)
            .ToListAsync(cancellationToken: cancellationToken);

        var typeFilteredVehicles = await _context.VehicleTypeFilters
            .Where(x => request.Types.Contains(x.VehicleTypeId))
            .Select(x => x.VehicleId)
            .ToListAsync(cancellationToken: cancellationToken);

        var intersectedVehiclesIds = extraServicesIncludedVehicles.Intersect(segmentFilteredVehicles).Intersect(typeFilteredVehicles);

        var vehicles = await _context.AccountVehicles
          //  .Where(x => intersectedVehiclesIds.Contains(x.Id))
          .Include(x => x.Vehicle)
          .ThenInclude(x=> x.VehicleModel)
          .Include(x => x.Vehicle)
          .ThenInclude(x => x.VehicleBrand)
          .Select(x => x.Vehicle)
            .ToListAsync(cancellationToken: cancellationToken);

        var vehicleCombinations = CalculateAllCombinations(vehicles, request.TotalPerson);
        var response = new GetAvailableVehiclesQueryResponse();
        
        foreach (var vehicleCombination in vehicleCombinations)
        {
            var totalPrice = vehicleCombination.VehicleUsage.Sum(keyValuePair => CalculatePriceOfVehicleUsage(keyValuePair.Key, keyValuePair.Value, request.TotalLengthOfRoad/1000));

            response.Add(new CombinationPricePair
            {
                Vehicles = vehicleCombination.VehicleUsage.Select(x => new VehicleUsagePair
                {
                    Usage = x.Value,
                    Vehicle = x.Key
                }).ToList(),
                TotalPrice = totalPrice
            });
        }

        return response;

    }
    
    decimal CalculatePriceOfVehicleUsage(Vehicle vehicle, int usage, decimal distance) => vehicle.BasePrice * distance * usage;


    static List<CombinationResult> CalculateAllCombinations(List<Vehicle> vehicles, int totalPeople)
    {
        // Kapasiteleri büyükten küçüğe doğru sıralama
        var sortedVehicles = vehicles.OrderByDescending(v => v.VehicleModel.TotalCapacity).ToList();

        // Tüm kombinasyonları tutmak için liste
        List<CombinationResult> combinations = new List<CombinationResult>();

        // Kombinasyonları hesaplayıcıyı çağır
        FindCombinations(sortedVehicles, totalPeople, new Dictionary<Vehicle, int>(), combinations);

        return combinations;
    }

    static void FindCombinations(List<Vehicle> vehicles, int remainingPeople, Dictionary<Vehicle, int> currentCombination, List<CombinationResult> allCombinations)
    {
        if (remainingPeople == 0)
        {
            // Eğer tam olarak kişi sayısına ulaştıysak, bu kombinasyonu kaydet
            allCombinations.Add(new CombinationResult(new Dictionary<Vehicle, int>(currentCombination), currentCombination.Sum(c => c.Value * c.Key.VehicleModel.TotalCapacity)));
            return;
        }

        if (remainingPeople < 0)
        {
            // Eğer kalan kişi sayısı negatif olduysa, bu kombinasyon geçersizdir, geri dön
            allCombinations.Add(new CombinationResult(new Dictionary<Vehicle, int>(currentCombination), currentCombination.Sum(c => c.Value * c.Key.VehicleModel.TotalCapacity)));

            return;
        }

        // Tüm araçları sırayla deneriz
        foreach (var vehicle in vehicles)
        {
            // Bu araç kombinasyona ekleniyor
            if (!currentCombination.ContainsKey(vehicle))
            {
                currentCombination[vehicle] = 0;
            }

            currentCombination[vehicle]++;  // Aracı bir kez daha kullanıyoruz

            // Yeni kalan kişi sayısı için recursive çağrı yapıyoruz
            FindCombinations(vehicles, remainingPeople - vehicle.VehicleModel.TotalCapacity, currentCombination, allCombinations);

            // Aracı geri çıkartıyoruz (backtrack)
            currentCombination[vehicle]--;

            if (currentCombination[vehicle] == 0)
            {
                currentCombination.Remove(vehicle);
            }
        }
    }
}