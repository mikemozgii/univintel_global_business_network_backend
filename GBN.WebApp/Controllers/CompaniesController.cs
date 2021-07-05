using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using UnivIntel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Models.DTO;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace GBN.WebApp.Controllers
{
    [Route("api/1/companies")]
    [ApiController]
    public class CompaniesController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly ISessionService m_SessionService;

        private readonly IAuthentificationService m_AuthentificationService;

        private readonly IEmailService m_EmailService;

        public CompaniesController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService, IAuthentificationService authentificationService, IEmailService emailService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            m_AuthentificationService = authentificationService ?? throw new ArgumentNullException(nameof(authentificationService));
            m_EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [Route("all")]
        public async Task<IEnumerable<Company>> GetCompanies() => await GetDatabase(m_QueryExecutionFactory).WhereAsync<Company>(QueryFilter.Equal(nameof(Company.AccountId), Session.Id));

        [Route("allaccounts")]
        public async Task<IEnumerable<object>> GetAccounts(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var companyAccounts = await database.KataQueryAsync<Account>(new Query().Where("IsEmployee", true).Where("EmployeeCompanyId", companyId));

            var accounts = new List<object>();

            foreach (var account in companyAccounts) accounts.Add(new { account.Id, Name = account.FirstName + " " + account.LastName, account.Email, account.AvatarId });

            return accounts;
        }

        [Route("signupemployee")]
        public async Task<string> SignupEmployee(string email, Guid companyId, string firstName, string lastName, string position, Guid? avatarId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var password = m_AuthentificationService.RandomPassword();
            m_AuthentificationService.SetDatabase(database);
            var error = await m_AuthentificationService.SignupEmployee(email, password, companyId, Session.Id, position, firstName, lastName, avatarId);
            if (!string.IsNullOrEmpty(error)) return "{\"error\": \"" + error + "\"}";

            await m_EmailService.SendSimpleEmail(email, "You was registered as employee in GBN service", $"Your password: {password}");

            return "{\"error\": \"\"}";
        }

        [Route("signleaccounts")]
        public async Task<object> SingleAccount(Guid companyId, Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var account = await database.FirstOrDefaultAsync<Account>(new Query().Where("Id", id).Where("EmployeeCompanyId", companyId));

            return new { account.FirstName, account.LastName, account.AvatarId, account.Position };
        }

        [Route("updateemployee")]
        public async Task<bool> UpdateEmployee(Guid companyId, string firstName, string lastName, string position, Guid? avatarId, Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var account = await database.FirstOrDefaultAsync<Account>(new Query().Where("Id", id).Where("EmployeeCompanyId", companyId));
            if (account == null) return false;

            account.FirstName = firstName;
            account.LastName = lastName;
            account.Position = position;
            account.AvatarId = avatarId;

            await database.UpdateAsync<Account>(account, new Query().Where("Id", id).Where("EmployeeCompanyId", companyId), exceptedFields: new List<string> { "Id", "Email", "TimeZoneId", "Password", "Language", "RankId", "IsEmployee", "EmployeeCompanyId" });

            return true;
        }

        [Route("changepassword")]
        public async Task<bool> ChangePassword(Guid companyId, string password, Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var account = await database.FirstOrDefaultAsync<Account>(new Query().Where("Id", id).Where("EmployeeCompanyId", companyId));
            if (account == null) return false;

            account.Password = m_AuthentificationService.EncodePassword(password);

            await database.UpdateAsync<Account>(account, new Query().Where("Id", id).Where("EmployeeCompanyId", companyId), exceptedFields: new List<string> { "Id", "Email", "TimeZoneId", "Language", "RankId", "IsEmployee", "EmployeeCompanyId", "FirstName", "LastName", "Position", "AvatarId" });

            return true;
        }

        [Route("deleteemployee")]
        public async Task<bool> DeleteEmployee(Guid companyId, Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            var account = await database.FirstOrDefaultAsync<Account>(new Query().Where("Id", id).Where("EmployeeCompanyId", companyId));
            if (account == null) return false;

            await database.KataQueryAsync<Account>(new Query().Where("Id", id).AsDelete());

            return true;
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddCompany([FromBody]Company company)
        {
            company.AccountId = Session.Id;

            if (await GetCompanyLimit())
            {
                await GetDatabase(m_QueryExecutionFactory).InsertAsync(company);
                return true;
            }
            else return false;
        }

        [Route("allowadd")]
        public async Task<bool> GetCompanyLimit()
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            var accountRank = await m_SessionService.GetAccountRank(Session.Id);

            var countQ = new Query("Companies").Where(nameof(Company.AccountId), "=", Session.Id).AsCount();

            var result = (await database.WhereAsync<CompanyCount>(countQ)).FirstOrDefault();

            return result.Count < accountRank.Companies;
        }

        [Route("mapsingle/{id}")]
        [HttpGet]
        public async Task<Company> MapSingleCompany(Guid id) => await GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService).FirstOrDefaultAsync<Company>(QueryFilter.Equal("Id", id));

        [Route("single/{id}")]
        [HttpGet]
        public async Task<Company> SingleCompany(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            var company = await database.FirstOrDefaultAsync<Company>(QueryFilter.Equal("Id", id));
            return company;
        }

        [Route("summary/{id}")]
        [HttpGet]
        public async Task<CompanySummary> CompanySummary(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            return await database.FirstOrDefaultAsync<CompanySummary>(QueryFilter.Equal("Id", id));
        }

        [Route("culture/{id}")]
        [HttpGet]
        public async Task<object> CompanyCulture(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            var culture = await database.FirstOrDefaultAsync<CompanyCulture>(new Query().Where("Id", id));
            var cultureImage = await database.FirstOrDefaultAsync<File>(new Query().Where("CompanyId", "=", id).Where("Tag", "cultureimage"));

            return new { culture, cultureImage };
        }

        [Route("common/{id}")]
        [HttpGet]
        public async Task<CompanyCommon> CompanyCommon(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            return await database.FirstOrDefaultAsync<CompanyCommon>(QueryFilter.Equal("Id", id));
        }

        [Route("annualfinancials/{id}")]
        [HttpGet]
        public async Task<object> CompanyAnnualFinancials(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            var annualFinancials = await database.FirstOrDefaultAsync<CompanyAnnualFinancials>(QueryFilter.Equal("Id", id));
            var financialsYears = await database.WhereAsync<CompanyFinancialYear>(new Query("CompanyFinancialYears").Where("CompanyId", "=", id).OrderBy("Year"));
            return new { annual = annualFinancials, years = financialsYears };
        }

        [Route("deleteannualfinancials")]
        [HttpGet]
        public async Task<bool> DeleteAnnualFinancials(Guid id, Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);
            await m_SessionService.KataCheckUserHaveAccessToSavedEntity<CompanyFinancialYear>(id, Session.Id);

            await database.KataQueryAsync<CompanyFinancialYear>(new Query().Where("CompanyId", companyId).Where("Id", id).AsDelete());

            return true;
        }


        [Route("documents/{id}")]
        [HttpGet]
        public async Task<IEnumerable<File>> CompanyDocuments(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            return await database.WhereAsync<File>(
                new Query("Files")
                    .Where("CompanyId", "=", id)
                    .Where(q =>
                        q.Where("Tag", "businessplan")
                            .OrWhere("Tag", "financialprojection")
                            .OrWhere("Tag", "companydocument")
                    )
            );
        }

        [Route("deletedocuments/{companyId}/{id}")]
        [HttpGet]
        public async Task<bool> DeleteCompanyDocuments(Guid companyId, Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            await database.KataQueryAsync<File>(
                new Query()
                    .Where("Id", "=", id)
                    .Where("CompanyId", "=", companyId)
                    .AsDelete()
            );

            return true;
        }

        [Route("pitchdeck/{id}")]
        [HttpGet]
        public async Task<IEnumerable<File>> CompanyPitchDeck(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            return await database.KataQueryAsync<File>(new Query().Where("CompanyId", "=", id).Where("Tag", "pitchdeck"));
        }

        [Route("fundinghistory/{id}")]
        [HttpGet]
        public async Task<object> FundingHistory(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            var roundNow = (await database.KataQueryAsync<CompanyFundingNow>(new Query().Where("Id", id).Limit(1))).FirstOrDefault();
            var history = await database.KataQueryAsync<FundingHistory>(new Query().Where("CompanyId", id));

            return new { now = roundNow, history = history.ToList() };
        }

        [Route("deletefundinghistory")]
        [HttpGet]
        public async Task<bool> DeleteFundingHistory(Guid id, Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);
            await m_SessionService.KataCheckUserHaveAccessToSavedEntity<FundingHistory>(id, Session.Id);

            await database.KataQueryAsync<FundingHistory>(new Query().Where("CompanyId", companyId).Where("Id", id).AsDelete());

            return true;
        }

        [Route("updatefundinghistory")]
        [HttpPost]
        public async Task<bool> UpdateAnnualFinancials(CompanyFundingNow fundingNow)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(fundingNow.Id, Session.Id);

            await database.UpdateAsync(fundingNow, QueryFilter.Equal("Id", fundingNow.Id));

            return true;
        }

        [Route("updateannualfinancials")]
        [HttpPost]
        public async Task<bool> UpdateAnnualFinancials(CompanyAnnualFinancials companySummary)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companySummary.Id, Session.Id);

            await database.UpdateAsync(companySummary, QueryFilter.Equal("Id", companySummary.Id));

            return true;
        }

        [Route("updateculture")]
        [HttpPost]
        public async Task<bool> UpdateCulture(CompanyCulture companyCulture)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyCulture.Id, Session.Id);

            await database.UpdateAsync(companyCulture, new Query().Where("Id", companyCulture.Id), new List<string> { "Id" });

            return true;
        }

        [Route("addorupdatefundinghistory")]
        [HttpPost]
        public async Task<bool> UpdateFundingHistory(FundingHistory fundingHistoryItem)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var isNew = fundingHistoryItem.Id == Guid.Empty;

            await m_SessionService.CheckUserHaveAccessToCompany(fundingHistoryItem.CompanyId, Session.Id);
            if (!isNew) await m_SessionService.CheckUserHaveAccessToSavedEntity<FundingHistory>(fundingHistoryItem.Id, Session.Id);

            fundingHistoryItem.AccountId = Session.Id;

            if (isNew)
            {
                await database.InsertAsync(fundingHistoryItem);
            }
            else
            {
                await database.UpdateAsync(fundingHistoryItem, QueryFilter.Equal("Id", fundingHistoryItem.Id));
            }

            return true;
        }

        [Route("addorupdatefinancialyear")]
        [HttpPost]
        public async Task<bool> UpdateAnnualFinancials(CompanyFinancialYear financialYear)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var isNew = financialYear.Id == Guid.Empty;

            await m_SessionService.CheckUserHaveAccessToCompany(financialYear.CompanyId, Session.Id);
            if (!isNew) await m_SessionService.CheckUserHaveAccessToSavedEntity<CompanyFinancialYear>(financialYear.Id, Session.Id);

            financialYear.AccountId = Session.Id;

            if (isNew)
            {
                await database.InsertAsync(financialYear);
            }
            else
            {
                await database.UpdateAsync(financialYear, QueryFilter.Equal("Id", financialYear.Id));
            }

            return true;
        }

        [Route("updatesummary/{id}")]
        [HttpPost]
        public async Task<bool> UpdateSummary(CompanySummary companySummary)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companySummary.Id, Session.Id);

            await database.UpdateAsync(companySummary, QueryFilter.Equal("Id", companySummary.Id));

            return true;
        }

        [Route("updatecommon/{id}")]
        [HttpPost]
        public async Task<bool> UpdateCommon([FromBody]CompanyCommon companyCommon)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyCommon.Id, Session.Id);

            await database.UpdateAsync(companyCommon, QueryFilter.Equal("Id", companyCommon.Id));

            return true;
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateCompany([FromBody]Company company)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(company.Id, Session.Id);

            company.AccountId = Session.Id;
            await database.UpdateAsync(company, new Query().Where("Id", company.Id), new List<string> { "CompanyRank" });
            return true;
        }

        [Route("deletecompany")]
        [HttpGet]
        public async Task<bool> DeleteCompany(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(id, Session.Id);

            await database.DeleteAsync<Company>(QueryFilter.Equal("Id", id));

            return true;
        }

        [Route("globalfilter")]
        [HttpPost]
        public async Task<IEnumerable<Company>> GlobalFilter(CompaniesFilterModel model)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            if (model.Ids?.Any() == true)
                return await database.KataQueryAsync<Company>(new Query().WhereIn(nameof(Company.Id), model.Ids));
            if(!string.IsNullOrEmpty(model.Name))
               return await database.KataQueryAsync<Company>(new Query().WhereLike(nameof(Company.Name), $"%{model.Name}%").Limit(5));

            return Enumerable.Empty<Company>();
        }

        [Route("companyitems")]
        [HttpGet]
        public async Task<object> GetCompanyItems(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            //TODO: add Rank conditions
            var date = DateTime.UtcNow;
            var productsAndServices = await database.KataQueryAsync<Product>(
                new Query()
                    .Where(nameof(Product.CompanyId), companyId)
                    .WhereTrue(nameof(Product.IsVisible))
                    .Where(q => q.WhereNull(nameof(Product.DateStart)).OrWhere(nameof(Product.DateStart), ">", date))
                    .Where(q => q.WhereNull(nameof(Product.DateEnd)).OrWhere(nameof(Product.DateEnd), "<", date))
                    .OrderBy((nameof(Product.Type))
                ));

            var discountsAndCoupons = await database.KataQueryAsync<Discount>(
                new Query()
                    .Where(nameof(Discount.CompanyId), companyId)
                    .WhereTrue(nameof(Discount.IsVisible))
                    .Where(q => q.WhereNull(nameof(Discount.DateStart)).OrWhere(nameof(Discount.DateStart), ">", date))
                    .Where(q => q.WhereNull(nameof(Discount.DateEnd)).OrWhere(nameof(Discount.DateEnd), "<", date))
                    .OrderBy((nameof(Discount.Type))
                ));

            var products = new List<Product>();
            var services = new List<Product>();
            foreach (var item in productsAndServices)
            {
                if (item.Type == "product") products.Add(item);
                if (item.Type == "service") services.Add(item);
            }

            var discounts = new List<Discount>();
            var coupons = new List<Discount>();
            foreach (var item in discountsAndCoupons)
            {
                if (item.Type == "discount") discounts.Add(item);
                if (item.Type == "coupon") coupons.Add(item);
            }

            return new 
            {
                products,
                services,
                discounts,
                coupons
            };
        }

    }

}