using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Interfaces;
using UnivIntel.GBN.Core.Models;
using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Services;

namespace UnivIntel.GBN.Core.Services
{
    public class CompanySvc : GBNDataAccessWorkerSvc
    {

        public CompanySvc(IGBNQueryExecution queryExecutionSvc) : base(queryExecutionSvc)
        {

        }


        //public async Task<string> GetConnectionStringByIdAsync(Guid dataSourceId)
        //{
        //    var dataSource = (await SelectByIdAsync<e_Company>(dataSourceId)).FirstOrDefault;

        //    return $"Host={dataSource.Host};Database={dataSource.Database};Username={dataSource.Username};Password={dataSource.Password}";
        //}

        //public async Task<QueryExecutionSvc> GetQueryExecutionSvcByIdAsync(Guid dataSourceId)
        //{
        //    var connStr = await GetConnectionStringByIdAsync(dataSourceId);
        //    return new QueryExecutionSvc(connStr);
        //}


        public async Task<QueryResponseGeneric<Company>> DeleteAsync(Guid id)
        {
            return await _GBNQueryExecutionSvc.DELETEAsync<Company>(id);
        }

        public async Task<QueryResponseGeneric<Company>> UpsertAsync(CompanyEditMdl m)
        {
            return !m.Id.HasValue ? await InsertAsync(m) : await UpdateAsync(m);
        }

        public async Task<QueryResponseGeneric<Company>> InsertAsync(CompanyEditMdl m)
        {
            var e = new Company()
            {
                Name = m.Name,
                Description = m.Description,
                //AccountId = await _CustomAuthStateProvider.GetAccountIdAsync()

            };

            return await _GBNQueryExecutionSvc.INSERTAsync(e);
        }

        public async Task<QueryResponseGeneric<Company>> UpdateAsync(CompanyEditMdl m)
        {

            return await _GBNQueryExecutionSvc.UPDATEAsync<Company>(m.Id.Value,
                nameof(Company.Name).Pair(m.Name),
                nameof(Company.Description).Pair(m.Description));

        }

    }
}
