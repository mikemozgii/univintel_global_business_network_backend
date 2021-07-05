using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Companies")]
    public class CompanyAnnualFinancials : GBNBaseRepository<CompanyAnnualFinancials>
    {

        [PgsPK]
        public Guid Id { get; set; }

        public long AnnualRevenueRunRate { get; set; }

        public long MonthlyBurnRate { get; set; }

        public string FinancialAnnotation { get; set; }

        public string RevenueDriver { get; set; }

    }
}
