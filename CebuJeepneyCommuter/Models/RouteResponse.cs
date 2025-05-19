using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.Models
{
    class RouteResponse
    {
        public string RouteId { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}
