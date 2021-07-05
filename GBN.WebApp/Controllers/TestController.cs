using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace GBN.WebApp.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        public TestController(IQueryExecutionFactory queryExecutionFactory)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
        }

        [Route("a")]
        public async Task Test ()
        {
            var database = GetDatabase(m_QueryExecutionFactory);
            var accounts = await database.FilterAsync<Account>(QueryFilter.And(QueryFilter.Equal("Email", "test@mail.ru")));
            if (accounts.Any())
            {

            }
        }

    }
}