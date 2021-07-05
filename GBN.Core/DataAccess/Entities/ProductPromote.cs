using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("ProductPromotes")]
    public class ProductPromote
    {
        public Guid ProductId { get; set; }
        public Guid PromoteId { get; set; }
    }
}
