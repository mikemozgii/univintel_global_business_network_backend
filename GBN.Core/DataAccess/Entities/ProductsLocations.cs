using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("ProductsLocations")]
    public class ProductsLocations : GBNBaseRepository<ProductsLocations>
    {
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public DateTime Date { get; set; }
    }
}
