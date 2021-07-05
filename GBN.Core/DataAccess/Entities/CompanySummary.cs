using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable(EntityAcceesName.Companies)]
    public class CompanySummary : GBNBaseRepository<CompanySummary>
    {
        [PgsPK]
        public Guid Id { get; set; }

        public string ManagementTeam { get; set; }

        public string CustomerProblem { get; set; }

        public string ProductsAndServices { get; set; }

        public string TargetMarket { get; set; }

        public string BusinessModel { get; set; }

        public string CustomerSegments { get; set; }

        public string SalesMarketingStrategy { get; set; }

        public string Competitors { get; set; }

        public string CompetitiveAdvantage { get; set; }
    }
}
