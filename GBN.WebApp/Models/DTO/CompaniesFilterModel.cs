using System;
using System.Collections.Generic;

namespace UnivIntel.GBN.WebApp.Models.DTO
{
    public class CompaniesFilterModel
    {
        public string Name { get; set; }
        public IEnumerable<Guid> Ids { get; set; }

    }

}
