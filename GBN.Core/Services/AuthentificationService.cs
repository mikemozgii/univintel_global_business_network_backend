using SqlKata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univintel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Handlers;
using UnivIntel.GBN.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Uuid;
using EntityTimeZone = UnivIntel.GBN.Core.DataAccess.Entities.TimeZone;

namespace UnivIntel.GBN.Core.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private const string m_PasswordSalt = "And so it was indeed: she was now only ten inches high, and her face brightened up at the thought that she was now the right size for going through the little door into that lovely garden. First, however, she waited for a few minutes to see if she was going to shrink any further: she felt a little nervous about this; ‘for it might end, you know,’ said Alice to herself, ‘in my going out altogether, like a candle. I wonder what I should be like then?’ And she tried to fancy what the flame of a candle is like after the candle is blown out, for she could not remember ever having seen such a thing.";

        private IDatabaseService m_DatabaseService;

        private readonly IBCryptProvider m_CryptProvider;

        private static ConcurrentDictionary<Guid, UserSession> m_Tokens = new ConcurrentDictionary<Guid, UserSession>();

        private static Random random = new Random();

        public AuthentificationService(IBCryptProvider cryptProvider)
        {
            m_CryptProvider = cryptProvider ?? throw new ArgumentNullException(nameof(cryptProvider));
        }

        public void SetDatabase(IDatabaseService databaseService)
        {
            m_DatabaseService = databaseService;
        }

        public string EncodePassword(string password) => m_CryptProvider.Hash(password, m_PasswordSalt);

        public async Task<string> Signup(string email, string timeZone)
        {
            var account = await m_DatabaseService.FilterAsync<Account>(QueryFilter.And(QueryFilter.Equal("Email", email)));
            var userAccount = account.FirstOrDefault();
            if (userAccount != null) return "Email already registered";

            var timeZones = await m_DatabaseService.FilterAsync<EntityTimeZone>(QueryFilter.And(QueryFilter.Equal("Name", timeZone)));
            var timeZoneEntity = timeZones.FirstOrDefault();
            if (timeZoneEntity == null) return "Time zone incorrect";

            await m_DatabaseService.InsertAsync(
                new Account
                {
                    Email = email,
                    TimeZoneId = timeZoneEntity.Id,
                    RankId = "new",
                    Language = "en",
                    IsEmployee = false
                }
            );

            return null;
        }

        public async Task<string> SignupEmployee(string email, string password, Guid companyId, Guid hostAccountId, string position, string firstName, string lastName, Guid? avatarId)
        {
            var hostAccount = await m_DatabaseService.FirstOrDefaultAsync<Account>(new Query().Where("Id", hostAccountId));

            var account = await m_DatabaseService.FilterAsync<Account>(QueryFilter.And(QueryFilter.Equal("Email", email)));
            var userAccount = account.FirstOrDefault();
            if (userAccount != null) return "Email already registered";

            await m_DatabaseService.InsertAsync(
                new Account
                {
                    Email = email,
                    Password = EncodePassword(password),
                    TimeZoneId = hostAccount.TimeZoneId,
                    RankId = "free",
                    FirstName = firstName,
                    LastName = lastName,
                    AvatarId = avatarId,
                    Position = position,
                    Language = hostAccount.Language,
                    IsEmployee = true,
                    EmployeeCompanyId = companyId
                }
            );

            return null;
        }

        public async Task<bool> VerifyEmail(string email, string code)
        {
            var codeQuery = new Query().Where("Email", "=", email).Where("Code", "=", code).Where("Date", ">=", DateTime.UtcNow.AddMinutes(-5)).Limit(1);
            var result = await m_DatabaseService.FirstOrDefaultAsync<VerifyCode>(codeQuery) != null;
            if (result)
            {
                await m_DatabaseService.UpdateAsync(new UpdateAccountRank { RankId = "free" }, new Query().Where("Email", "=", email), new List<string> { "Id" });
            }
            return result;
        }

        public async Task<string> ResetPassword(Guid accountId, string oldPassword, string newPassword)
        {
            var userAccount = await m_DatabaseService.FirstOrDefaultAsync<Account>(new Query().Where("Id", "=", accountId));

            if (userAccount == null) return "account is not found";

            if (!m_CryptProvider.IsValid(oldPassword, userAccount.Password, m_PasswordSalt)) return "old password is not correct";
            userAccount.Password = EncodePassword(newPassword);

            var exceptedFields = typeof(Account).GetProperties().Where(i => i.Name != "Password").Select(s => s.Name).ToList();
            await m_DatabaseService.UpdateAsync(userAccount, new Query().Where("Id", userAccount.Id), exceptedFields);
            //TODO: send notification about changed password
            return "";
        }

        public string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<(string, Guid)> Signin(string email, string code)
        {
            var userAccount = await m_DatabaseService.FirstOrDefaultAsync<Account>(new Query().Where("Email", "=", email));

            if (userAccount == null) return ("incorrect_data", Guid.Empty);

            var codeQuery = new Query().Where("Email", "=", email).Where("Code", "=", code).Where("Date", ">=", DateTime.UtcNow.AddMinutes(-5)).Limit(1);
            var result = await m_DatabaseService.FirstOrDefaultAsync<VerifyCode>(codeQuery) != null;

            if(!result) return ("incorrect_data", Guid.Empty);

            var expires = DateTime.Now.AddDays(20);

            var userSession = new UserSession
            {
                Id = userAccount.Id,
                Expires = expires
            };


            var createdItem = await m_DatabaseService.InsertAsync(new Tokens
            {
                //Token = token, ORM fill this field automatically even if it already filled
                AccountId = userAccount.Id,
                Expires = expires
            });

            m_Tokens.TryAdd(createdItem.Token, userSession);

            return ("", createdItem.Token);
        }

        public async Task<bool> Signout(Guid token)
        {
            if (!m_Tokens.TryRemove(token, out var session)) return false;

            await m_DatabaseService.DeleteAsync<Tokens>(QueryFilter.Equal("Token", token));

            return true;
        }

        public static UserSession GetUserSession(Guid token) => m_Tokens.ContainsKey(token) ? m_Tokens[token] : null;

        public static async Task<UserSession> GetOrRaiseExpireUserSession(Guid token)
        {
            var tokenInMemory = GetUserSession(token);
            if (tokenInMemory == null) return null;

            if ((tokenInMemory.Expires - DateTime.Now).TotalDays > 2) return tokenInMemory;

            tokenInMemory.Expires = DateTime.Now.AddDays(20);

            var entity = new Tokens
            {
                Token = token,
                AccountId = tokenInMemory.Id,
                Expires = tokenInMemory.Expires
            };
            await new DatabaseService(GlobalSettings.ConnStr, "sessionraiseexpires" + tokenInMemory.Id).UpdateAsync(entity, new Query().Where("Token", token), new List<string> { "Token" });

            return tokenInMemory;
        }

        public static void FillTokens()
        {
            var factory = new QueryExecutionFactory();
            var database = factory.GetDatabase(new UserSession { Id = Guid.Empty });

            var tokens = database.Filter<Tokens>(QueryFilter.Greather("Expires", DateTime.Now));
            foreach (var token in tokens)
            {
                m_Tokens.TryAdd(
                    token.Token,
                    new UserSession
                    {
                        Id = token.AccountId,
                        Expires = token.Expires
                    }
                );
            }
        }

        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < 12; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString().ToLower();
        }
    }

}
