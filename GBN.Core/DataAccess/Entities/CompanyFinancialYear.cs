using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{

    [PgsDataTable("CompanyFinancialYears")]
    public class CompanyFinancialYear : GBNBaseRepository<CompanyFinancialYear>
    {

        [PgsPK]
        public Guid Id { get; set; }

        public int Year { get; set; }

        public string RevenueDriver { get; set; }

        public int Revenue { get; set; }

        public int Expenditure { get; set; }

        public Guid CompanyId { get; set; }

        public Guid AccountId { get; set; }

    }
}
