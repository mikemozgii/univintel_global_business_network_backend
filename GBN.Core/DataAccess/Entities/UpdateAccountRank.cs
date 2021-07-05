using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable(EntityAcceesName.Accounts)]
    public class UpdateAccountRank
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string RankId { get; set; }
    }

}
