using System;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.Interfaces;
using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.GBN.Core.Services
{
    public class GBNDataAccessWorkerSvc : IGBNDataAccessWorker
    {
        //private IGBNQueryExecution queryExecutionSvc;
        public IGBNQueryExecution _GBNQueryExecutionSvc { get; set; }

        //public CustomAuthStateProvider _CustomAuthStateProvider { get; set; }
        public GBNDataAccessWorkerSvc(IGBNQueryExecution queryExecutionSvc)
        {
            _GBNQueryExecutionSvc = queryExecutionSvc;
            //_CustomAuthStateProvider = _customAuthStateProvider;


        }


        //public async Task<QueryResponseGeneric<T>> SelectByAccountIdAsync<T>() where T : BaseRepository<T>, new()
        //{
        //    var accountId = await _CustomAuthStateProvider.GetAccountIdAsync();
        //    return await _GBNDQueryExecutionSvc.SELECTbyColumnValueAsync<T>(Pairing.Of(nameof(e_DataSource.AccountId), accountId));
        //}

        //public async Task<QueryResponseGeneric<T>> SelectByClusterIdAsync<T>(Guid clusterId) where T : BaseRepository<T>, new()
        //{
        //    return await _GBNDQueryExecutionSvc.SELECTbyColumnValueAsync<T>(Pairing.Of(nameof(e_Host.ClusterId), clusterId));
        //}

        public async Task<QueryResponseGeneric<T>> SelectByIdAsync<T>(Guid id) where T : BaseRepository<T>, new()
        {
            return await _GBNQueryExecutionSvc.SELECTbyIdAsync<T>(id);
        }
    }
}
