using Microsoft.AspNetCore.Mvc;
using System;
using Univintel.GBN.Core;
using UnivIntel.GBN.Core.Models;
using UnivIntel.GBN.Core.Services;

namespace GBN.WebApp.Controllers
{
    public class SessionController : Controller
    {

        public UserSession Session => AuthentificationService.GetUserSession(Guid.Parse(HttpContext.Items["PageSession"].ToString()));

        protected IDatabaseService GetDatabase(IQueryExecutionFactory queryExecutionFactory) => queryExecutionFactory.GetDatabase(Session);

        protected IDatabaseService GetDatabaseWithSession(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            var database = queryExecutionFactory.GetDatabase(Session);
            
            sessionService.SetDatabase(database);
            
            return database;
        }

    }
}