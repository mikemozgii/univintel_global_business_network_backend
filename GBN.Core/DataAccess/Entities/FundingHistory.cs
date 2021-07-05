using System;
using System.Collections.Generic;
using System.Text;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{

    [PgsDataTable("CompanyFundingHistory")]
    public class FundingHistory : GBNBaseRepository<FundingHistory>
    {

        [PgsPK]
        public Guid Id { get; set; }

        public string Round { get; set; }

        public long CapitalRaised { get; set; }

        public DateTime ClosingDate { get; set; }

        public string InvestorName { get; set; }

        public string InvestorEmail { get; set; }

        public Guid CompanyId { get; set; }

        public Guid AccountId { get; set; }

    }
}
