using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable(EntityAcceesName.Companies)]
    public class CompanyCount : GBNBaseRepository<CompanyCount>
    {
        public int Count { get; set; }
    }
}
