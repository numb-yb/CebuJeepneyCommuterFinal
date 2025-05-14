using System.Collections.Generic;
using System.Linq;
using CebuJeepneyCommuter.Models;
using System;

namespace CebuJeepneyCommuter.Services
{
    public static class RouteDataService
    {
        public static List<RouteInfo> GetAllRoutes()
        {
            return new List<RouteInfo>
            {
                new RouteInfo { Code = "09G", Origin = "Basak", Destination = "Colon", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "10H", Origin = "Bulacao", Destination = "SM via Colon", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "10M", Origin = "Bulacao", Destination = "Ayala via Jones Ave", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "12L", Origin = "Labangon", Destination = "Carbon via Jones", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "12D", Origin = "Labangon", Destination = "Colon", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "01K", Origin = "Urgello", Destination = "IT Park via Jones", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "13C", Origin = "Lahug", Destination = "Colon via Jones", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "06B", Origin = "Guadalupe", Destination = "Carbon", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "06H", Origin = "Guadalupe", Destination = "SM via Mango", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "62B", Origin = "Minglanilla", Destination = "Carbon", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "62C", Origin = "Minglanilla", Destination = "SM City Cebu", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "61B", Origin = "Tabunok", Destination = "Carbon", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "61D", Origin = "Tabunok", Destination = "Ayala via SRP", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "10F", Origin = "Bulacao", Destination = "Ayala via SRP", Type = "Jeepney", Classification = "Economy" },
                new RouteInfo { Code = "04C", Origin = "Lahug", Destination = "Carbon via Jones Ave", Type = "Jeepney", Classification = "Regular" },
                new RouteInfo { Code = "MB01", Origin = "Talisay (SM Seaside)", Destination = "SM City - IT Park", Type = "MyBus", Classification = "Economy" },
                new RouteInfo { Code = "MB02", Origin = "Minglanilla", Destination = "SM City Cebu", Type = "MyBus", Classification = "Economy" },
                new RouteInfo { Code = "BP01", Origin = "Minglanilla", Destination = "Ayala via SRP", Type = "Beep", Classification = "Economy" },
                new RouteInfo { Code = "BP02", Origin = "Talisay", Destination = "IT Park via SM City", Type = "Beep", Classification = "Economy" },
                new RouteInfo { Code = "BP03", Origin = "Bulacao", Destination = "Ayala via Colon", Type = "Beep", Classification = "Economy" },
                new RouteInfo { Code = "BP04", Origin = "Lawaan", Destination = "SM City - Ayala", Type = "Beep", Classification = "Economy" },
                new RouteInfo { Code = "BP05", Origin = "Minglanilla", Destination = "Colon", Type = "Beep", Classification = "Economy" }
            };
        }

        public static RouteInfo FindRoute(string origin, string destination)
        {
            return GetAllRoutes().FirstOrDefault(r => r.Origin.Equals(origin, StringComparison.OrdinalIgnoreCase)
                                                       && r.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase));
        }
    }
}
