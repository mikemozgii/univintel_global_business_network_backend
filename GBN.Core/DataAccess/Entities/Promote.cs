using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Promotes")]
    public class Promote
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; }
        public string ItemTypeId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid AccountId { get; set; }
    }
}
