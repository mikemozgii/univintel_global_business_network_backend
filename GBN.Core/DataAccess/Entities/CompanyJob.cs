using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Jobs")]
    public class CompanyJob
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Locations { get; set; }
        public Guid? ContactId { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string TypePosition { get; set; }
        public string Skills { get; set; }
        public string WorkedExperience { get; set; }
        public Guid CompanyId { get; set; }
        public Guid AccountId { get; set; }
    }

}
