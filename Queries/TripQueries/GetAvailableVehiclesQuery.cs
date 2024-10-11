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
    public List<GetAvailableVehiclesQueryHandler.VehicleUsagePair> Vehicles { get; set; }
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

        var vehicles = await _context.Vehicles
          .Include(x=> x.VehicleModel)
          .Include(x => x.VehicleBrand)
          .OrderBy(x => x.VehicleModel.Capacity)
          .Reverse()
            .ToListAsync(cancellationToken: cancellationToken);

        var vehicleCombinations = CalculateCombinations(vehicles, request.TotalPerson);
        var response = new GetAvailableVehiclesQueryResponse();
        
        foreach (var vehicleCombination in vehicleCombinations)
        {
            var totalPrice = vehicleCombination.Vehicles.Sum(x => CalculatePriceOfVehicleUsage(x.Vehicle, x.Usage, request.TotalLengthOfRoad/1000));

            response.Add(new CombinationPricePair
            {
                Vehicles = vehicleCombination.Vehicles.Select(x => new VehicleUsagePair
                {
                    Usage = x.Usage,
                    Vehicle = x.Vehicle
                }).ToList(),
                TotalPrice = totalPrice
            });
        }

        return response;

    }
    
    decimal CalculatePriceOfVehicleUsage(Vehicle vehicle, int usage, decimal distance) => vehicle.BasePrice * distance * usage;

    // VehicleUsagePair sınıfı
    public class VehicleUsagePair
    {
        public Vehicle Vehicle { get; set; }
        public int Usage { get; set; }
    }
    
     // Combination sınıfı
    public class Combination
    {
        public List<VehicleUsagePair> Vehicles { get; set; } = new List<VehicleUsagePair>();

        // Aynı kombinasyonları kontrol edebilmek için, araçları sıralı tutacağız
        public override bool Equals(object obj)
        {
            var other = obj as Combination;
            if (other == null) return false;

            // Aynı araç kombinasyonları olmalı
            if (Vehicles.Count != other.Vehicles.Count) return false;

            // Araç ve kullanım sayısına göre karşılaştırma yapıyoruz
            return Vehicles.OrderBy(v => v.Vehicle.Id).SequenceEqual(other.Vehicles.OrderBy(v => v.Vehicle.Id));
        }

        public override int GetHashCode()
        {
            // Sıralı şekilde hash hesaplanacak
            return Vehicles.OrderBy(v => v.Vehicle.Id)
                           .Select(v => v.Vehicle.Id.GetHashCode() ^ v.Usage) // Araç GUID ve kullanımı birleştiriyoruz
                           .Aggregate(0, (a, b) => a ^ b); // XOR operatörü ile hash hesaplaması
        }
    }

    // Kombinasyonları hesaplayan fonksiyon
    static List<Combination> CalculateCombinations(List<Vehicle> vehicles, int passengerCount)
    {
        HashSet<Combination> results = new HashSet<Combination>();

        // Recursive fonksiyon
        FindCombinations(vehicles, passengerCount, new Dictionary<Guid, int>(), 0, results);

        return results.ToList();
    }

    static void FindCombinations(List<Vehicle> vehicles, int passengersLeft, Dictionary<Guid, int> currentCombination, int startIndex, HashSet<Combination> results)
    {
        // Eğer tüm yolcuları taşıyabilecek bir kombinasyon bulduysak sonuçlara ekle
        if (passengersLeft <= 0)
        {
            Combination combination = new Combination();
            foreach (var entry in currentCombination)
            {
                Vehicle vehicle = vehicles.Find(v => v.Id == entry.Key);
                combination.Vehicles.Add(new VehicleUsagePair
                {
                    Vehicle = vehicle,
                    Usage = entry.Value
                });
            }

            // Sıralı ve unique olacak şekilde kombinasyonları ekliyoruz
            results.Add(combination);
            return;
        }

        // Araçları sırayla deneyelim
        for (int i = startIndex; i < vehicles.Count; i++)
        {
            var vehicle = vehicles[i];

            // Her araç için maksimum kaç tane gerekebileceğini hesapla
            int maxVehicleCount = (int)Math.Ceiling((double)passengersLeft / vehicle.VehicleModel.Capacity);

            // Mevcut kombinasyona araç ekleyelim
            if (!currentCombination.ContainsKey(vehicle.Id))
            {
                currentCombination[vehicle.Id] = 0;
            }

            // Bu araç için 1'den maxVehicleCount'a kadar kombinasyonları deneyelim
            currentCombination[vehicle.Id] += 1;

            FindCombinations(vehicles, passengersLeft - vehicle.VehicleModel.Capacity, currentCombination, i, results);

            // Geri al (backtrack)
            currentCombination[vehicle.Id] -= 1;

            // Eğer mevcut araç sayısı 0'a düşerse, o aracı kombinasyondan kaldır
            if (currentCombination[vehicle.Id] == 0)
            {
                currentCombination.Remove(vehicle.Id);
            }
        }
    }
    
    
    /*
    
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
    */
}