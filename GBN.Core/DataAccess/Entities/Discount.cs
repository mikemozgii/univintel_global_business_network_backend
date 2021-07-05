using System;
using System.Collections.Generic;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Discounts")]
    public class Discount : GBNBaseRepository<Discount>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStart { get; set; }
        public Guid AccountId { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsVisible { get; set; }
        public bool Reusable { get; set; }
        public string OnlyCompanies { get; set; }
        public int MinRank { get; set; }
    }
}
