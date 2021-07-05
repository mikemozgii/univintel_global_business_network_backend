using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GBN.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;

namespace UnivIntel.GBN.WebApp.Controllers
{
    [Route("api/1/jobs")]
    [ApiController]
    public class JobsController : SessionController
    {

        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly ISessionService m_SessionService;

        public JobsController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("all")]
        public async Task<IEnumerable<CompanyJob>> GetJobs(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            return await database.KataQueryAsync<CompanyJob>(
                new Query()
                    .Where("AccountId", Session.Id)
                    .Where("CompanyId", companyId)
            );
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddJob([FromBody]CompanyJob job)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(job.CompanyId, Session.Id);

            job.AccountId = Session.Id;
            if (job.Locations == null) job.Locations = "[]";
            await database.AddAsync(job, new List<string> { "Id" });
            return true;
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateJob([FromBody]CompanyJob job)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.KataCheckUserHaveAccessToSavedEntity<CompanyJob>(job.Id, Session.Id);
            await m_SessionService.CheckUserHaveAccessToCompany(job.CompanyId, Session.Id);

            job.AccountId = Session.Id;
            if (job.Locations == null) job.Locations = "[]";
            await database.UpdateAsync(job, new Query().Where("Id", job.Id), new List<string> { "Id" });
            return true;
        }

        [Route("delete")]
        [HttpGet]
        public async Task<bool> DeleteJob(Guid id, Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.KataCheckUserHaveAccessToSavedEntity<CompanyJob>(id, Session.Id);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            await database.KataQueryAsync<CompanyJob>(new Query().Where("Id", id).AsDelete());

            return true;
        }

    }

}