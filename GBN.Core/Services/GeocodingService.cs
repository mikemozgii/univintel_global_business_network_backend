using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.Models.Geocoding;

namespace UnivIntel.GBN.Core.Services
{
    public class GeocodingService : IGeocodingService
    {
        public async Task<IEnumerable<GeocodingResult>> GeocodeForAddress(string address)
        {
            var response = await new HttpClient().GetStringAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key=AIzaSyBp1oefBlrMcF8vxwyoNHQpaZq1-RUQXok");
            var geocodingResults = JsonConvert.DeserializeObject<GoogleGeocoding>(response);

            if (geocodingResults.Status != "OK") return null;

            return geocodingResults.Results;
        }
    }
}
