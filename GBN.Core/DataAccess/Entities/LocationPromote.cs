using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("LocationPromotes")]
    public class LocationPromote
    {
        public Guid LocationId { get; set; }
        public Guid PromoteId { get; set; }
    }
}
