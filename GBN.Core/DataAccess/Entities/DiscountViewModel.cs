using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Discounts")]
    public class DiscountViewModel : GBNBaseRepository<DiscountViewModel>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Title { get; set; }
        //public string Type { get; set; }
        public string Description { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ImageId { get; set; }
    }
}
