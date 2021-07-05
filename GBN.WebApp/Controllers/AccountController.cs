using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnivIntel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Handlers;
using UnivIntel.GBN.Core.Models;
using UnivIntel.GBN.Core.Services;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace GBN.WebApp.Controllers
{
    [Route("api/1/account")]
    [ApiController]
    public class AccountController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;
        private readonly IAuthentificationService m_AuthentificationService;
        private readonly IEmailService m_EmailService;

        public AccountController(IQueryExecutionFactory queryExecutionFactory, IAuthentificationService authentificationService, IEmailService emailService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_AuthentificationService = authentificationService ?? throw new ArgumentNullException(nameof(authentificationService));
            m_EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [Route("information")]
        [HttpGet]
        public async Task<IActionResult> Information()
        {
            var database = GetDatabase(m_QueryExecutionFactory);
            var account = await database.FirstOrDefaultAsync<Account>(QueryFilter.Equal("Id", Session.Id));

            return new JsonResult(new {
                account.Email, 
                Name = $"{account.FirstName} {account.LastName}", 
                account.LastName,
                account.FirstName,
                AvatarId = account.AvatarId.HasValue ? account.AvatarId.ToString() : "",
                account.Language,
                account.IsEmployee,
                account.EmployeeCompanyId,
                account.Bio,
                account.RankId
            });
        }

        [Route("changerank")]
        [HttpGet]
        public async Task<bool> ChangeRank(string rank)
        {
            if (!GlobalSettings.AccountRanks.ContainsKey(rank)) return false;
            var database = GetDatabase(m_QueryExecutionFactory);

            var account = new AccountRankUpdateModel
            {
                RankId = rank,
                RankDateEnd = DateTime.UtcNow.AddYears(1)
            };

            await database.UpdateAsync(account, new SqlKata.Query().Where("Id", Session.Id));

            return true;
        }

        [Route("changelanguage")]
        [HttpGet]
        public async Task<bool> ChangeLanguage(string language)
        {
            var database = GetDatabase(m_QueryExecutionFactory);
            var account = await database.FirstOrDefaultAsync<Account>(QueryFilter.Equal("Id", Session.Id));

            account.Language = language;

            await database.UpdateAsync(account, QueryFilter.Equal("Id", Session.Id));

            return true;
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Information([FromBody]Account accountModel)
        {
            var database = m_QueryExecutionFactory.GetDatabase(
                new UserSession
                {
                    Id = new Guid("AC95C886-DD3F-410B-BAA3-C2F93C1EC51D")
                }
            );
            var account = await database.FirstOrDefaultAsync<Account>(QueryFilter.Equal("Id", Session.Id));
            account.LastName = accountModel.LastName;
            account.FirstName = accountModel.FirstName;
            account.Bio = accountModel.Bio;
            if(!string.IsNullOrEmpty(accountModel.Email))
                account.Email = accountModel.Email;
            var res = await database.UpdateAsync(account, QueryFilter.Equal("Id", Session.Id));

            return new JsonResult(new { Result = true });
        }

        [Route("checkemail")]
        [HttpGet]
        public async Task<string> CheckEmail([FromQuery]string email, [FromQuery]string languageCode)
        {
            var database = GetDatabase(m_QueryExecutionFactory);
            var account = await database.FilterAsync<Account>(QueryFilter.And(QueryFilter.Equal("Email", email)));
            var userAccount = account.FirstOrDefault();
            if (userAccount != null) return "Email already registered";
                var code = m_AuthentificationService.RandomString(6);
            var codeModel = new VerifyCode { Id = Guid.Empty, Email = email, Code = code };
            await database.AddAsync(codeModel, new List<string> { "Id" });

            var templateExists = !string.IsNullOrEmpty(languageCode) && System.IO.File.Exists($"EmailTemplates\\Verification_{languageCode}.html");
            var templateFile = templateExists ? $"Verification_{languageCode}.html" : $"Verification_en.html";
            var body = System.IO.File.ReadAllText($"EmailTemplates\\{templateFile}");
            var subject = templateExists && languageCode == "ru" ? "Ваш код подтверждения в сервисе Univintel" : "Your Univintel Verification Code";
            await m_EmailService.SendSimpleEmail(email, subject, body.Replace("[code]", code));
#if DEBUG
            Debug.WriteLine($"Try signin code for email {email}: {code}");
#endif
            return"";
        }


        [Route("avatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody]Account accountModel)
        {
            var database = GetDatabase(m_QueryExecutionFactory);
            var account = await database.FirstOrDefaultAsync<Account>(QueryFilter.Equal("Id", Session.Id));
            account.AvatarId = accountModel.AvatarId;
            await database.UpdateAsync<Account>(account, QueryFilter.Equal("Id", Session.Id));

            return new JsonResult(new { Result = true });
        }

        [Route("resetpassword")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword)) return BadRequest("not defined password");
            if (string.IsNullOrEmpty(newPassword)) return BadRequest("not defined new pasword");

            m_AuthentificationService.SetDatabase(GetDatabase(m_QueryExecutionFactory));
            var result = await m_AuthentificationService.ResetPassword(Session.Id, oldPassword, newPassword);
            if(!string.IsNullOrEmpty(result)) BadRequest(result);

            return Ok();
        }

    }
}