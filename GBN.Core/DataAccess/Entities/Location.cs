using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Locations")]
    public class Location : GBNBaseRepository<Location>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public Guid? CityId { get; set; }
        public Guid AccountId { get; set; }
        public Guid CompanyId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Visible { get; set; }
        public string WorkingHours { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
    }
}
