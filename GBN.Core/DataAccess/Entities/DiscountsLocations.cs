using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("DiscountsLocations")]
    public class DiscountsLocations : GBNBaseRepository<DiscountsLocations>
    {
        public Guid DiscountId { get; set; }
        public Guid LocationId { get; set; }
        public DateTime Date { get; set; }
    }
}
