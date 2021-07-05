using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Companies")]
    public class CompanyCulture
    {
        public Guid Id { get; set; }

        public string Culture { get; set; }

        public string CultureVideoLink { get; set; }

    }
}
