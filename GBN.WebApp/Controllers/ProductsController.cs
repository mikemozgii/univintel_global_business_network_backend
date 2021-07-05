using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GBN.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using SqlKata.Execution;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Models.DTO;

namespace UnivIntel.GBN.WebApp.Controllers
{
    [Route("api/1/products")]
    [ApiController]
    public class ProductsController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;
        private readonly ISessionService m_SessionService;

        public ProductsController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("all")]
        public async Task<IEnumerable<Product>> GetItems(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var q = new Query("Products").Where(nameof(Product.CompanyId), "=", companyId).OrderBy(nameof(Product.Title)); 

            return await database.KataQueryAsync<Product>(q);
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddItem([FromBody]ProductModel item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            item.DateCreated = DateTime.UtcNow;
            if (item.ImageId.HasValue && item.ImageId == Guid.Empty) item.ImageId = null;
            var product = await database.AddAsync(item, new List<string> { "Id", nameof(ProductModel.AddressesIds) });

            if (product.Id != null) await UpdateLocations(item.AddressesIds, product.Id, database);
            return true;
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateItem([FromBody]ProductModel item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            if (item.ImageId.HasValue && item.ImageId == Guid.Empty) item.ImageId = null;
            await database.UpdateAsync(item, new Query().Where(nameof(Product.Id), item.Id), new List<string> { "Id", nameof(ProductModel.AddressesIds) });

            await UpdateLocations(item.AddressesIds, item.Id, database);
            return true;
        }

        [Route("delete")]
        [HttpGet]
        public async Task<bool> DeleteItem(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var item = await m_SessionService.GetSavedEntityByAccount<Product>(id, Session.Id);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            await database.KataQueryAsync<Product>(new Query("Products").Where(nameof(Product.Id), item.Id).AsDelete());

            return true;
        }

        [Route("locations")]
        public async Task<IEnumerable<Guid>> GetLocations(Guid id, Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var q = new Query("ProductsLocations")
                .Join("Products", $"Products.{nameof(Product.Id)}", $"ProductsLocations.{nameof(ProductsLocations.ProductId)}")
                .Where($"Products.{nameof(Product.CompanyId)}", "=", companyId)
                .Where($"Products.{nameof(Product.Id)}", "=", id);

            return (await database.WhereAsync<ProductsLocations>(q)).Select(s => s.LocationId).ToList();
        }

        private async Task UpdateLocations(IEnumerable<Guid> addressesIds, Guid itemId, Univintel.GBN.Core.IDatabaseService database)
        {
            await database.KataQueryAsync<ProductsLocations>(new Query().Where(nameof(ProductsLocations.ProductId), itemId).AsDelete());

            var length = addressesIds?.Any();
            if (!length.HasValue || length.HasValue && length.Value == false) return;

            var exceptedFields = new List<string> { "Date" };
            var addressesTasks =
                addressesIds.Select(addressId => database.AddAsync(
                    new ProductsLocations { LocationId = addressId, ProductId = itemId },
                    exceptedFields));
            await Task.WhenAll(addressesTasks);
        }
    }
}