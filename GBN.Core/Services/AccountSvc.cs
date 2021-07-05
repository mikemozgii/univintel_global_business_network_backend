using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Interfaces;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.GBN.Core.Services
{
    public class AccountSvc: GBNDataAccessWorkerSvc
    {
        public AccountSvc(IGBNQueryExecution queryExecutionSvc) : base(queryExecutionSvc)
        {

        }



        public async Task<QueryResponseGeneric<Account>> SELECTbyIdAsync(Guid accountId)
        {
            return await _GBNQueryExecutionSvc.SELECTbyIdAsync<Account>(accountId);
        }
    }
}
