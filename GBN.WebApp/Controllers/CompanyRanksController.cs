using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;

namespace GBN.WebApp.Controllers
{
    [Route("api/1/companyranks")]
    [ApiController]
    public class CompanyRanksController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly ISessionService m_SessionService;

        public CompanyRanksController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("all")]
        public async Task<IEnumerable<CompanyRank>> GetRanks() => await GetDatabase(m_QueryExecutionFactory).KataQueryAsync<CompanyRank>(new Query().OrderBy("Id"));

        [Route("setrank")]
        public async Task<bool> SetRank(int rank, Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var company = await database.FirstOrDefaultAsync<Company>(new Query().Where("Id", companyId));
            company.CompanyRank = rank;
            await database.UpdateAsync(company, new Query().Where("Id", companyId), new List<string> { "Id" });

            return true;
        }

    }

}