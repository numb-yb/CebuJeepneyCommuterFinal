
namespace CebuJeepneyCommuter.Models
{
    public class RouteInfo
    {
        public string Code { get; set; } // e.g., 09G, MB01, BP01
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Type { get; set; } // e.g., Jeepney, MyBus, Beep
        public string Classification { get; set; } // e.g., Aircon, Non-Aircon
    }
}