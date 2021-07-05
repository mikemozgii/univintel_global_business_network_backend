using System;
using System.Collections.Generic;
using System.Text;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("VerifyCodes")]
    public class VerifyCode
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Code { get; set; }
    }
}
