using System.Collections.Generic;

namespace UnivIntel.GBN.Core.Models.Geocoding
{

    public class GoogleGeocoding
    {
        public IEnumerable<GeocodingResult> Results { get; set; }

        public string Status { get; set; }
    }

}
