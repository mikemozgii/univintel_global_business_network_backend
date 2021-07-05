using Univintel.GBN.Core;
using UnivIntel.GBN.Core.Handlers;
using UnivIntel.GBN.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Services;

namespace UnivIntel.GBN.Core.Services
{
    public class QueryExecutionFactory : IQueryExecutionFactory
    {
        public IDatabaseService GetDatabase(UserSession userSession)
        {
            return new DatabaseService(GlobalSettings.ConnStr, GlobalSettings.GetAppName(userSession.Id));
        }

        public QueryExecutionSvc GetQueryExecutionService(UserSession userSession)
        {
            return new QueryExecutionSvc(GlobalSettings.ConnStr, GlobalSettings.GetAppName(userSession.Id));
        }
    }
}
