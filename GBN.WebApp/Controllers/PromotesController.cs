using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GBN.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Models.DTO;

namespace UnivIntel.GBN.WebApp.Controllers
{
    [Route("api/1/promotes")]
    [ApiController]
    public class PromotesController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;
        private readonly ISessionService m_SessionService;

        public PromotesController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddItem([FromBody]PromoteModel itemModel)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            var item = itemModel.Item;

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);
            item.AccountId = Session.Id;
            item.DateCreated = DateTime.UtcNow;
            var savedItem = await database.AddAsync(item, new List<string> { "Id" });

            if (item.ItemTypeId == "product")
            {
                var pp = new ProductPromote { ProductId = itemModel.EntityId, PromoteId = savedItem.Id };
                await database.AddAsync(pp);
            }
            else if (item.ItemTypeId == "location")
            {
                var pp = new LocationPromote { LocationId = itemModel.EntityId, PromoteId = savedItem.Id };
                await database.AddAsync(pp);
            }
            else if (item.ItemTypeId == "discount")
            {
                var pp = new DiscountPromote { DiscountId = itemModel.EntityId, PromoteId = savedItem.Id };
                await database.AddAsync(pp);
            }

            return true;
        }

        [Route("byfilter")]
        [HttpPost]
        public async Task<IEnumerable<Promote>> GetPromotes(PromotesFilterModel filterModel)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(filterModel.CompanyId, Session.Id);

            var q = new Query().Where(nameof(Promote.CompanyId), filterModel.CompanyId);
            if (!string.IsNullOrEmpty(filterModel.ItemType))
                q = q.Where(nameof(Promote.ItemTypeId), filterModel.ItemType);
            if (filterModel.ItemId.HasValue)
                q = q.Where(nameof(Promote.Id), filterModel.ItemId.Value);

            return await database.KataQueryAsync<Promote>(q);
        }

        [Route("types")]
        [HttpGet]
        public async Task<IEnumerable<PromoteTypes>> GetPromoteTypes()
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var q = new Query()
                .Where(nameof(PromoteTypes.DateStart), "<", DateTime.UtcNow)
                .Where(c => c.WhereNull(nameof(PromoteTypes.DateEnd)).OrWhere(nameof(PromoteTypes.DateEnd), ">", DateTime.UtcNow)
            );

            return await database.KataQueryAsync<PromoteTypes>(q);
        }

    }
}