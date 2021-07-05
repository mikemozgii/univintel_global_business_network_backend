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
    [Route("api/1/discounts")]
    [ApiController]
    public class DiscountsController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;
        private readonly ISessionService m_SessionService;

        public DiscountsController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("all")]
        public async Task<IEnumerable<Discount>> GetItems(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var q = new Query("Discounts").Where(nameof(Discount.CompanyId), "=", companyId).OrderBy(nameof(Discount.Title));

            return await database.WhereAsync<Discount>(q);
        }

        [Route("byfilter")]
        [HttpPost]
        public async Task<IEnumerable<DiscountViewModel>> GetLocations(DiscountFilterModel filterModel)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var q = new Query("Discounts").SelectRaw("DISTINCT ON (\"Discounts\".\"Id\") *")
                .Join("DiscountsLocations", $"Discounts.{nameof(Discount.Id)}", $"DiscountsLocations.{nameof(DiscountsLocations.DiscountId)}");
            if (filterModel.LocationsIds?.Any() == true)
                q = q.WhereIn($"DiscountsLocations.{nameof(DiscountsLocations.LocationId)}", filterModel.LocationsIds);

            return await database.WhereAsync<DiscountViewModel>(q);
        }

        [Route("locations")]
        public async Task<IEnumerable<Guid>> GetDiscountsByLocations(Guid id, Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var q = new Query("DiscountsLocations")
                .Join("Discounts", $"Discounts.{nameof(Discount.Id)}", $"DiscountsLocations.{nameof(DiscountsLocations.DiscountId)}")
                .Where($"Discounts.{nameof(Discount.CompanyId)}", "=", companyId)
                .Where($"Discounts.{nameof(Discount.Id)}", "=", id);

            return (await database.WhereAsync<DiscountsLocations>(q)).Select(s => s.LocationId).ToList();
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddItem([FromBody]DiscountModel item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            item.DateCreated = DateTime.UtcNow;
            if (item.ImageId.HasValue && item.ImageId == Guid.Empty) item.ImageId = null;
            var discount = await database.AddAsync(item, new List<string> { "Id", nameof(DiscountModel.AddressesIds) });

            if (discount.Id != null) await UpdateLocations(item.AddressesIds, discount.Id, database);



            return true;
        }

        private async Task UpdateLocations(IEnumerable<Guid> addressesIds, Guid itemId, Univintel.GBN.Core.IDatabaseService database)
        {
            await database.KataQueryAsync<DiscountsLocations>(new Query().Where(nameof(DiscountsLocations.DiscountId), itemId).AsDelete());

            var length = addressesIds?.Any();
            if (!length.HasValue || length.HasValue && length.Value == false) return;

            var exceptedFields = new List<string> { "Date" };
            var addressesTasks =
                addressesIds.Select(addressId => database.AddAsync(
                    new DiscountsLocations { LocationId = addressId, DiscountId = itemId },
                    exceptedFields));
            await Task.WhenAll(addressesTasks);
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateItem([FromBody]DiscountModel item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;

            if (item.ImageId.HasValue && item.ImageId.Value == default(Guid)) item.ImageId = null;

            await database.UpdateAsync(item, new Query().Where(nameof(Discount.Id), item.Id), new List<string> { "Id", nameof(DiscountModel.AddressesIds) });

            await UpdateLocations(item.AddressesIds, item.Id, database);
            return true;
        }

        [Route("delete")]
        [HttpGet]
        public async Task<bool> DeleteItem(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var item = await m_SessionService.GetSavedEntityByAccount<Discount>(id, Session.Id);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            await database.KataQueryAsync<Discount>(new Query("Discounts").Where(nameof(Discount.Id), item.Id).AsDelete());

            return true;
        }
    }
}