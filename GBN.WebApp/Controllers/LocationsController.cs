using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlKata;
using Univintel.GBN.Core;
using UnivIntel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Models.Geocoding;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Models;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace GBN.WebApp.Controllers
{
    [Route("api/1/locations")]
    [ApiController]
    public class LocationsController : SessionController
    {

        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly ISessionService m_SessionService;

        private readonly IGeocodingService m_GeocodingService;

        public LocationsController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService, IGeocodingService geocodingService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            m_GeocodingService = geocodingService ?? throw new ArgumentNullException(nameof(geocodingService));
        }


        [Route("all")]
        public async Task<IEnumerable<Location>> GetAll(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            return await database.WhereAsync<Location>(
                QueryFilter.Equal(nameof(Location.AccountId), Session.Id),
                QueryFilter.Equal(nameof(Location.CompanyId), companyId)
            );
        }

        [Route("cities")]
        public async Task<IEnumerable<CityWithNames>> GetCities(string filter)
        {
            return await GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService)
                .KataQueryAsync<CityWithNames>(
                    new Query()
                        .Join("Countries", "Countries.Id", "Cities.CountryId")
                        .WhereLike("Cities.Name", $"{filter.ToLowerInvariant()}%")
                        .Select("Countries.Name as Country", "Cities.Name as City")
                );
        }

        private async Task SaveCoordinates(Location location, IDatabaseService databaseService)
        {
            var results = await m_GeocodingService.GeocodeForAddress($"{location.PostalCode} {location.City} {location.Line1} {location.Line2}");
            if (results == null || !results.Any()) return;

            var geocodingResult = results.FirstOrDefault();
            await databaseService.SaveLocationGeometry(location.Id, geocodingResult.Geometry.Location.Lat, geocodingResult.Geometry.Location.Lng);
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddItem([FromBody]Location item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            await database.InsertAsync(item);

            await SaveCoordinates(item, database);

            return true;
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateItem([FromBody]Location item)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToSavedEntity<Location>(item.Id, Session.Id);
            await m_SessionService.CheckUserHaveAccessToCompany(item.CompanyId, Session.Id);

            item.AccountId = Session.Id;
            await database.UpdateAsync(item, QueryFilter.Equal("Id", item.Id));

            await SaveCoordinates(item, database);

            return true;
        }

        [Route("workinghours")]
        [HttpGet]
        public async Task<string> GetWorkingHours(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var location = await database.FirstOrDefaultAsync<Location>(new Query().Where("Id", id));
            if (location == null) throw new ArgumentException("Incorrect identifier.");

            return string.IsNullOrEmpty(location.WorkingHours) ? "[]" : location.WorkingHours;
        }

        [Route("delete")]
        [HttpGet]
        public async Task<bool> DeleteItem(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            var location = await m_SessionService.GetSavedEntityByAccount<Location>(id, Session.Id);

            await m_SessionService.CheckUserHaveAccessToCompany(location.CompanyId, Session.Id);

            await database.DeleteAsync<Location>(QueryFilter.Equal("Id", id));

            return true;
        }

        [Route("nearlocations")]
        [HttpGet]
        public async Task<IEnumerable<MapLocation>> NearLocations(double latitude, double longitude)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            var point = $"'SRID=4326;POINT({latitude} {longitude})'::geometry";

            var discountQuery = new Query("DiscountsLocations as discountlocation");
            discountQuery
                .Select("promotetype.Color")
                .Join("DiscountPromotes as promotec", "promotec.DiscountId", "discountlocation.DiscountId")
                .Join("Promotes as promote", "promote.Id", "promotec.PromoteId")
                .Join("PromoteTypes as promotetype", "promotetype.Id", "promote.TypeId")
                .Join("Discounts as discount", "discountlocation.DiscountId", "discount.Id")
                .WhereColumns("Locations.Id", "=", "discountlocation.LocationId")
                .Where("discount.IsVisible", true)
                .Where(q => q.WhereNull("promote.DateEnd").OrWhereRaw("promote.\"DateEnd\" > now()") )
                .Where(q => q.WhereNull("promote.DateStart").OrWhereRaw("promote.\"DateStart\" < now()") )
                .OrderByDesc("discount.MinRank")
                .Limit(1);

            var productQuery = new Query("ProductsLocations as productlocation");
            productQuery
                .Select("promotetype.Color")
                .Join("ProductPromotes as promoteb", "promoteb.ProductId", "productlocation.ProductId")
                .Join("Promotes as ppromote", "ppromote.Id", "promoteb.PromoteId")
                .Join("PromoteTypes as promotetype", "promotetype.Id", "ppromote.TypeId")
                .Join("Products as product", "productlocation.ProductId", "product.Id")
                .WhereColumns("Locations.Id", "=", "productlocation.LocationId")
                .Where("product.IsVisible", true)
                .Where(q => q.WhereNull("ppromote.DateEnd").OrWhereRaw("ppromote.\"DateEnd\" > now()") )
                .Where(q => q.WhereNull("ppromote.DateStart").OrWhereRaw("ppromote.\"DateStart\" < now()") )
                .OrderByDesc("product.MinRank")
                .Limit(1);

            var query = new Query()
                    .Join("Companies", "Locations.CompanyId", "Companies.Id")
                    .WhereRaw($"ST_Distance(\"Locations\".\"Point\", {point}) < 5")
                    .Where($"Companies.DisplayCompany", true)
                    .Where($"Locations.Visible", true)
                    .Select(new string[] {
                        "Locations.Id as Id",
                        "Locations.Name as Name",
                        "Companies.Name as Company",
                        "Companies.Id as CompanyId",
                        "Locations.Category as Category",
                    })
                    .Select(discountQuery, "DiscountType")
                    .Select(productQuery, "ProductType")
                    .SelectRaw("ST_AsText(\"Point\") AS \"Position\"");
            var locations = await database.KataQueryAsync<MapLocation>(query);

            locations.AsParallel().ForAll(a =>
            {
                var coordinates = a.Position.Replace("POINT(", "").Replace(")", "").Split(" ");
                a.Latitude = Convert.ToDouble(coordinates[0]);
                a.Longitude = Convert.ToDouble(coordinates[1]);
            });

            return locations;
        }

        [Route("companylocations")]
        [HttpGet]
        public async Task<IEnumerable<MapLocation>> CompanyLocations(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            var locations = await database.KataQueryAsync<MapLocation>(
                new Query()
                    .Join("Companies", "Locations.CompanyId", "Companies.Id")
                    .Where("Locations.CompanyId", companyId)
                    .Select(new string[] { "Locations.Id as Id", "Locations.Name as Name", "Companies.Name as Company" })
                    .SelectRaw("ST_AsText(\"Point\") AS \"Position\"")
            );

            locations.AsParallel().ForAll(a =>
            {
                var coordinates = a.Position.Replace("POINT(", "").Replace(")", "").Split(" ");
                a.Latitude = Convert.ToDouble(coordinates[0]);
                a.Longitude = Convert.ToDouble(coordinates[1]);
            });

            return locations;
        }

    }

}