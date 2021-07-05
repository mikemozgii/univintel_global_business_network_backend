using Univintel.GBN.Core;
using UnivIntel.GBN.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Services;

namespace UnivIntel.GBN.Core.Services
{
    public interface IQueryExecutionFactory
    {
        QueryExecutionSvc GetQueryExecutionService(UserSession userSession);

        IDatabaseService GetDatabase(UserSession userSession);
    }
}