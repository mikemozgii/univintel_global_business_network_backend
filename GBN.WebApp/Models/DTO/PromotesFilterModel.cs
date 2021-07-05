using System;

namespace UnivIntel.GBN.WebApp.Models.DTO
{
    public class PromotesFilterModel
    {
        public string ItemType { get; set; }
        public Guid? ItemId { get; set; }
        public Guid CompanyId { get; set; }
    }

}
