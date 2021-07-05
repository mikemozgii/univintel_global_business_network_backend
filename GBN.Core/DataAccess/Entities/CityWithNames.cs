using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{

    [PgsDataTable("Cities")]
    public class CityWithNames
    {

        public string City { get; set; }

        public string Country { get; set; }

    }
}
