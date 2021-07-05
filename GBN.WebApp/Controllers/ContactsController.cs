using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Services;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace GBN.WebApp.Controllers
{
    [Route("api/1/contacts")]
    [ApiController]
    public class ContactsController : SessionController
    {

        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly ISessionService m_SessionService;

        public ContactsController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("all")]
        public async Task<IEnumerable<Contact>> GetCompanies(Guid companyId)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(companyId, Session.Id);

            return await database.WhereAsync<Contact>(
                QueryFilter.Equal(nameof(Contact.AccountId), Session.Id),
                QueryFilter.Equal(nameof(Contact.CompanyId), companyId)
            );
        }

        [Route("add")]
        [HttpPost]
        public async Task<bool> AddContact([FromBody]Contact contact)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            await m_SessionService.CheckUserHaveAccessToCompany(contact.CompanyId, Session.Id);

            contact.AccountId = Session.Id;
            contact.Name = "Company contact"; //May be make this on client????
            await database.InsertAsync(contact);
            return true;
        }

        [Route("update")]
        [HttpPost]
        public async Task<bool> UpdateContact([FromBody]Contact contact)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
            
            await m_SessionService.CheckUserHaveAccessToSavedEntity<Contact>(contact.Id, Session.Id);
            await m_SessionService.CheckUserHaveAccessToCompany(contact.CompanyId, Session.Id);

            contact.AccountId = Session.Id;
            contact.Name = "Company contact"; //May be make this on client????
            await database.UpdateAsync(contact, QueryFilter.Equal("Id", contact.Id));
            return true;
        }

        [Route("delete")]
        [HttpGet]
        public async Task<bool> DeleteContact(Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var contact = await m_SessionService.GetSavedEntityByAccount<Contact>(id, Session.Id);

            await m_SessionService.CheckUserHaveAccessToCompany(contact.CompanyId, Session.Id);

            await database.DeleteAsync<Contact>(QueryFilter.Equal("Id", id));

            return true;
        }

    }

}