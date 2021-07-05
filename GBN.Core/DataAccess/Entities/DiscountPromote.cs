using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("DiscountPromotes")]
    public class DiscountPromote
    {
        public Guid DiscountId { get; set; }
        public Guid PromoteId { get; set; }
    }
}
