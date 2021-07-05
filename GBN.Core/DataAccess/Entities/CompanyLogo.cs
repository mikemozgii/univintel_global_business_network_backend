using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable(EntityAcceesName.Companies)]
    public class CompanyLogo : GBNBaseRepository<CompanyLogo>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid? LogoId { get; set; }
    }
}
