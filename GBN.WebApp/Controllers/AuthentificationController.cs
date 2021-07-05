using Microsoft.AspNetCore.Mvc;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using UnivIntel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Models;
using UnivIntel.GBN.Core.Services;

namespace GBN.WebApp.Controllers
{
    [Route("api/1/authentification")]
    [ApiController]
    public class AuthentificationController : Controller
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        private readonly IAuthentificationService m_AuthentificationService;

        private readonly IEmailService m_EmailService;

        public AuthentificationController(IQueryExecutionFactory queryExecutionFactory, IAuthentificationService authentificationService, IEmailService emailService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_AuthentificationService = authentificationService ?? throw new ArgumentNullException(nameof(authentificationService));
            m_EmailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        private bool IsEmailValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Sign up user.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        [Route("signup")]
        public async Task<string> Signup(string email, string timeZone, string languageCode)
        {
            if (string.IsNullOrEmpty(timeZone)) return "not defined timezone";
            if (string.IsNullOrEmpty(email)) return "not defined email";
            if (!IsEmailValid(email)) return "incorrect email";

            m_AuthentificationService.SetDatabase(
                m_QueryExecutionFactory.GetDatabase(
                    new UserSession
                    {
                        Id = Guid.Empty
                    }
                )
            );

            var result = await m_AuthentificationService.Signup(email, timeZone);

            if (!string.IsNullOrEmpty(result)) return result;

            var database = m_QueryExecutionFactory.GetDatabase(
                new UserSession
                {
                    Id = new Guid("AC95C886-DD3F-410B-BAA3-C2F93C1EC51D")
                }
            );

            var code = m_AuthentificationService.RandomString(6);
            var codeModel = new VerifyCode { Id = Guid.Empty, Email = email, Code = code };
            await database.AddAsync(codeModel, new List<string> { "Id" });

            var templateExists = !string.IsNullOrEmpty(languageCode) && System.IO.File.Exists($"EmailTemplates\\Verification_{languageCode}.html");
            var templateFile = templateExists ? $"Verification_{languageCode}.html" : $"Verification_en.html";
            var body = System.IO.File.ReadAllText($"EmailTemplates\\{templateFile}");
            var subject = templateExists && languageCode == "ru" ? "Ваш код подтверждения в сервисе Univintel" : "Your Univintel Verification Code";
            await m_EmailService.SendSimpleEmail(email, subject, body.Replace("[code]", code));

            return string.Empty;
        }

        [Route("trysignin")]
        public async Task<string> TrySignIn(string email, string languageCode)
        {
            if (string.IsNullOrEmpty(email)) return "not defined email";

            var database = m_QueryExecutionFactory.GetDatabase(
                new UserSession
                {
                    Id = new Guid("AC95C886-DD3F-410B-BAA3-C2F93C1EC51D")
                }
            );

            var userAccount = await database.FirstOrDefaultAsync<Account>(new Query().Where("Email", "=", email));

            if (userAccount == null) return "incorrect_data";


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
            return string.Empty;
        }




        [Route("verify")]
        public async Task<IActionResult> VerifyEmail(string email, string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest("not defined code");
            if (string.IsNullOrEmpty(email)) return BadRequest("not defined email");

            m_AuthentificationService.SetDatabase(
                m_QueryExecutionFactory.GetDatabase(
                    new UserSession
                    {
                        Id = Guid.Empty
                    }
                )
            );

            return Ok(await m_AuthentificationService.VerifyEmail(email, code));
        }

        /// <summary>
        /// Sign up user.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        [Route("signin")]
        public async Task<IActionResult> Signin(string email, string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest("not defined code");
            if (string.IsNullOrEmpty(email)) return BadRequest("not defined email");

            m_AuthentificationService.SetDatabase(
                m_QueryExecutionFactory.GetDatabase(
                    new UserSession
                    {
                        Id = Guid.Empty
                    }
                )
            );

            var (error, token) = await m_AuthentificationService.Signin(email, code);

            if (string.IsNullOrEmpty(error)) return Content(token.ToString());
            return BadRequest(error);
        }

        [Route("signout")]
        public async Task<bool> Signout()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("GBNToken")) return false;

            var token = Guid.Parse(HttpContext.Request.Cookies["GBNToken"]);
            var userSession = AuthentificationService.GetUserSession(token);
            if (userSession == null) return true;

            m_AuthentificationService.SetDatabase(m_QueryExecutionFactory.GetDatabase(userSession));

            return await m_AuthentificationService.Signout(token);
        }

        [Route("forgotpassword")]
        [HttpGet]
        public async Task<bool> ForgotPassword(string email)
        {
            var database = m_QueryExecutionFactory.GetDatabase(
                new UserSession
                {
                    Id = new Guid("AC95C886-DD3F-410B-BAA3-C2F93C1EC51D")
                }
            );

            m_AuthentificationService.SetDatabase(database);

            var account = await database.FirstOrDefaultAsync<Account>(new Query().WhereRaw("lower([Email]) = ?", email.ToLowerInvariant()));
            if (account == null) return true;

            var randomPassword = m_AuthentificationService.RandomPassword();
            account.Password = m_AuthentificationService.EncodePassword(randomPassword);

            await database.UpdateAsync(account, new Query().AsUpdate(account).Where("Id", account.Id));

            await m_EmailService.SendSimpleEmail(account.Email, "You new password", "You new password " + randomPassword);

            return true;
        }

    }

}