using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GBN.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.GBN.WebApp.Controllers
{
    [Route("api/1/news")]
    [ApiController]
    public class NewsController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly ISessionService m_SessionService;

        public NewsController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("all")]
        public async Task<IEnumerable<News>> GetItems(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            return await database.WhereAsync<News>(
                QueryFilter.Equal(nameof(News.AccountId), Session.Id),
                QueryFilter.Equal(nameof(News.CompanyId), companyId)
            );
        }

        [Route("allonmap")]
        public async Task<IEnumerable<object>> GetItemsForMap(Guid companyId)
        {
            var news = await GetDatabase(m_QueryExecutionFactory)
                .KataQueryAsync<News>(
                    new Query()
                        .Where("CompanyId", companyId)
                        .Where("Visible", true)
                        .Where("DatePublish", "<", DateTime.UtcNow)
                        .OrderByDesc("DatePublish")
                );

            return news
                .Select(
                    a => new {
                        a.Id,
                        a.Title,
                        a.Body,
                        a.Link,
                        a.ImageId
                    }
                )
                .ToList();
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddItem([FromBody]News item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            item.DateCreated = DateTime.UtcNow;

            await database.InsertAsync(item);
            return true;
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateItem([FromBody]News item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToSavedEntity<News>(item.Id, Session.Id);
            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            
            await database.UpdateAsync(item, new SqlKata.Query().Where("Id", item.Id), new List<string> { "Id" });
            return true;
        }

        [Route("delete")]
        [HttpGet]
        public async Task<bool> DeleteItem(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var item = await m_SessionService.GetSavedEntityByAccount<News>(id, Session.Id);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            await database.DeleteAsync<News>(QueryFilter.Equal("Id", id));

            return true;
        }
    }
}