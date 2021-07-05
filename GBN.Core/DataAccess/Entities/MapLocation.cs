using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Locations")]
    public class MapLocation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public Guid CompanyId { get; set; }
        public string Category { get; set; }
        public int Level { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Position { get; set; }
        public string DiscountType { get; set; }
        public string ProductType { get; set; }
    }
}
