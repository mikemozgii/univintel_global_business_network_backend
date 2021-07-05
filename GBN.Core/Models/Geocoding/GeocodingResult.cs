using System.Collections.Generic;

namespace UnivIntel.GBN.Core.Models.Geocoding
{
    public class GeocodingResult
    {
        public List<AddressComponent> AddressComponents { get; set; }
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public string PlaceId { get; set; }
        public PlusCode PlusCode { get; set; }
        public List<string> Types { get; set; }
    }

}
