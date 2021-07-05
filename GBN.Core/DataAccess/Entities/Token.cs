using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{

    [PgsDataTable(EntityAcceesName.Tokens)]
    public class Tokens : GBNBaseRepository<Tokens>
    {

        [PgsPK]
        public Guid Token { get; set; }

        public Guid AccountId { get; set; }

        public DateTime Expires { get; set; }

    }

}
