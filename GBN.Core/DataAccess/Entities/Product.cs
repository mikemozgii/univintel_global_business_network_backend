using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Products")]
    public class Product : GBNBaseRepository<Product>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Upc { get; set; }
        public decimal Price { get; set; }
        public decimal CurrentPrice { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsVisible { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid AccountId { get; set; }
        public string OnlyCompanies { get; set; }
        public int MinRank { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStart { get; set; }
    }
}
