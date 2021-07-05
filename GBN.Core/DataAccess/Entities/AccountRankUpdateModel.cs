using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable(EntityAcceesName.Accounts)]
    public class AccountRankUpdateModel
    {
        public DateTime? RankDateEnd { get; set; }
        public string RankId { get; set; }
        
    }

}
