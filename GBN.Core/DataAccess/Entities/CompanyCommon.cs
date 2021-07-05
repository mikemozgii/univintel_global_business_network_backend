using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Companies")]
    public class CompanyCommon : GBNBaseRepository<CompanyCommon>
    {

        [PgsPK]
        public Guid Id { get; set; }

        public string OneLinePitch { get; set; }

        public string IncorporationType { get; set; }

        public string CompanyStage { get; set; }

        public string PitchVideoLink { get; set; }

    }
}
