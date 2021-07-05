using System;
using System.Collections.Generic;
using UnivIntel.GBN.Core.DataAccess.Entities;

namespace UnivIntel.GBN.WebApp.Models.DTO
{
    public class DiscountModel : Discount
    {
        public IEnumerable<Guid> AddressesIds { get; set; }
    }
}
