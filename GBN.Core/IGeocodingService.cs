using System.Collections.Generic;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.Models.Geocoding;

namespace UnivIntel.GBN.Core
{
    public interface IGeocodingService
    {

        Task<IEnumerable<GeocodingResult>> GeocodeForAddress(string address);

    }
}
