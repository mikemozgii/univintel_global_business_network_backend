using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable(EntityAcceesName.Companies)]
    public class Company : GBNBaseRepository<Company>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid AccountId { get; set; }
        public DateTime? Founded { get; set; }
        public string Tagline { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Linkedin { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public int? CompanySize { get; set; }
        public int? Industry { get; set; }
        public string Abbreviation { get; set; }
        public string Email { get; set; }
        public bool DisplayCompany { get; set; }
        public bool DisplayInvestmentPortfolio { get; set; }
        public Guid? LogoId { get; set; }
        public int CompanyRank { get; set; }
    }
}
