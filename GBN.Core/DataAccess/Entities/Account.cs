using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{

    [PgsDataTable(EntityAcceesName.Accounts)]
    public class Account : GBNBaseRepository<Account>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid TimeZoneId { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public Guid? AvatarId { get; set; }
        public DateTime? RankDateEnd { get; set; }
        public string RankId { get; set; }
        public bool IsEmployee { get; set; }
        public Guid? EmployeeCompanyId { get; set; }
        public string Position { get; set; }
        public string Bio { get; set; }
    }

}
