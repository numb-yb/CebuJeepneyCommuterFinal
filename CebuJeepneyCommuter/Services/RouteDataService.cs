using CebuJeepneyCommuter.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CebuJeepneyCommuter.Services;


namespace CebuJeepneyCommuter.Services
{
    public static class RouteDataService
    {
        private static readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "routes.json");

        // Async load routes
        public static async Task<List<RouteInfo>> GetAllRoutesAsync()
        {
            if (!File.Exists(filePath))
            {
                var defaultRoutes = GetDefaultRoutes();
                await SaveRoutesAsync(defaultRoutes); // Save default on first load
                return defaultRoutes;
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<RouteInfo>>(json);
        }

        // Async save routes
        public static async Task SaveRoutesAsync(List<RouteInfo> routes)
        {
            var json = JsonSerializer.Serialize(routes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }

        // Async find route
        public static async Task<RouteInfo> FindRouteAsync(string origin, string destination)
        {
            var routes = await GetAllRoutesAsync();
            return routes.FirstOrDefault(r =>
                r.Origin.Equals(origin, System.StringComparison.OrdinalIgnoreCase) &&
                r.Destination.Equals(destination, System.StringComparison.OrdinalIgnoreCase));
        }
        public static async Task<bool> RouteCodeExistsAsync(string code)
        {
            var routes = await GetAllRoutesAsync();
            return routes.Any(r => r.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }



        // Your default route data
        private static List<RouteInfo> GetDefaultRoutes() => new()
        {
            new RouteInfo { Code = "09G", Origin = "Basak", Destination = "Colon", Type = "Jeepney", Classification = "Regular", RegularFare = 13.00m },
            new RouteInfo { Code = "10H", Origin = "Bulacao", Destination = "SM via Colon", Type = "Jeepney", Classification = "Economy", RegularFare = 12.00m },
            new RouteInfo { Code = "10M", Origin = "Bulacao", Destination = "Ayala via Jones Ave", Type = "Jeepney", Classification = "Regular", RegularFare = 14.00m },
            new RouteInfo { Code = "12L", Origin = "Labangon", Destination = "Carbon via Jones", Type = "Jeepney", Classification = "Regular", RegularFare = 13.00m },
            new RouteInfo { Code = "12D", Origin = "Labangon", Destination = "Colon", Type = "Jeepney", Classification = "Economy", RegularFare = 12.00m },
            new RouteInfo { Code = "01K", Origin = "Urgello", Destination = "IT Park via Jones", Type = "Jeepney", Classification = "Economy", RegularFare = 13.00m },
            new RouteInfo { Code = "13C", Origin = "Lahug", Destination = "Colon via Jones", Type = "Jeepney", Classification = "Regular", RegularFare = 13.00m },
            new RouteInfo { Code = "06B", Origin = "Guadalupe", Destination = "Carbon", Type = "Jeepney", Classification = "Regular", RegularFare = 13.00m },
            new RouteInfo { Code = "06H", Origin = "Guadalupe", Destination = "SM via Mango", Type = "Jeepney", Classification = "Economy", RegularFare = 13.00m },
            new RouteInfo { Code = "62B", Origin = "Minglanilla", Destination = "Carbon", Type = "Jeepney", Classification = "Regular", RegularFare = 15.00m },
            new RouteInfo { Code = "62C", Origin = "Minglanilla", Destination = "SM City Cebu", Type = "Jeepney", Classification = "Economy", RegularFare = 14.00m },
            new RouteInfo { Code = "61B", Origin = "Tabunok", Destination = "Carbon", Type = "Jeepney", Classification = "Regular", RegularFare = 14.00m },
            new RouteInfo { Code = "61D", Origin = "Tabunok", Destination = "Ayala via SRP", Type = "Jeepney", Classification = "Economy", RegularFare = 15.00m },
            new RouteInfo { Code = "10F", Origin = "Bulacao", Destination = "Ayala via SRP", Type = "Jeepney", Classification = "Economy", RegularFare = 14.00m },
            new RouteInfo { Code = "04C", Origin = "Lahug", Destination = "Carbon via Jones Ave", Type = "Jeepney", Classification = "Regular", RegularFare = 13.00m },
            new RouteInfo { Code = "MB01", Origin = "Talisay (SM Seaside)", Destination = "SM City - IT Park", Type = "MyBus", Classification = "Economy", RegularFare = 30.00m },
            new RouteInfo { Code = "MB02", Origin = "Minglanilla", Destination = "SM City Cebu", Type = "MyBus", Classification = "Economy", RegularFare = 25.00m },
            new RouteInfo { Code = "BP01", Origin = "Minglanilla", Destination = "Ayala via SRP", Type = "Beep", Classification = "Economy", RegularFare = 20.00m },
            new RouteInfo { Code = "BP02", Origin = "Talisay", Destination = "IT Park via SM City", Type = "Beep", Classification = "Economy", RegularFare = 20.00m },
            new RouteInfo { Code = "BP03", Origin = "Bulacao", Destination = "Ayala via Colon", Type = "Beep", Classification = "Economy", RegularFare = 15.00m },
            new RouteInfo { Code = "BP04", Origin = "Lawaan", Destination = "SM City - Ayala", Type = "Beep", Classification = "Economy", RegularFare = 18.00m },
            new RouteInfo { Code = "BP05", Origin = "Minglanilla", Destination = "Colon", Type = "Beep", Classification = "Economy", RegularFare = 18.00m }
        };
    }
}
