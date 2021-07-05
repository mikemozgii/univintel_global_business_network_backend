using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Companies")]
    public class CompanyFundingNow : GBNBaseRepository<CompanyFundingNow>
    {

        [PgsPK]
        public Guid Id { get; set; }

        public string Round { get; set; }

        public int Seeking { get; set; }

        public string SecurityType { get; set; }

    }
}
