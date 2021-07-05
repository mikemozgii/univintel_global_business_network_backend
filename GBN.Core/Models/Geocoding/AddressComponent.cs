using System.Collections.Generic;

namespace UnivIntel.GBN.Core.Models.Geocoding
{
    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

}
