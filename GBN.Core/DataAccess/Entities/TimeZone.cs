using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{

    [PgsDataTable(EntityAcceesName.TimeZones)]
    public class TimeZone : GBNBaseRepository<TimeZone>
    {
        [PgsPK]
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        public string Name { get; set; }

        public long GMTOffset { get; set; }

        public long DSTOffset { get; set; }

        public long RAWOffset { get; set; }

    }

}
