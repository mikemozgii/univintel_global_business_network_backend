using System;
using UnivIntel.GBN.Core.DataAccess.Entities;

namespace UnivIntel.GBN.WebApp.Models.DTO
{
    public class PromoteModel
    {
        public Promote Item { get; set; }
        public Guid EntityId { get; set; }
        //public Guid CompanyId { get; set; }
    }
    
}
