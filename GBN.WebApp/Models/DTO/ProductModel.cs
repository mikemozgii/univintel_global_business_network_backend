using System;
using System.Collections.Generic;
using UnivIntel.GBN.Core.DataAccess.Entities;

namespace UnivIntel.GBN.WebApp.Models.DTO
{
    public class ProductModel : Product
    {
        public IEnumerable<Guid> AddressesIds { get; set; }
    }
}
