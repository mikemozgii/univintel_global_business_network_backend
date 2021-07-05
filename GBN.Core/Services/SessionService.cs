using SqlKata;
using System;
using System.Linq;
using System.Threading.Tasks;
using Univintel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Handlers;
using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.GBN.Core.Services
{
    public class SessionService : ISessionService
    {
        private IDatabaseService m_DatabaseService;

        public async Task CheckUserHaveAccessToCompany(Guid companyId, Guid accountId)
        {
            var company = await m_DatabaseService.FirstOrDefaultAsync<Company>(QueryFilter.Equal("Id", companyId), QueryFilter.Equal("AccountId", accountId));
            if (company == null) throw new ArgumentException("Account don't have this company.");
        }

        public async Task CheckUserHaveAccessToSavedEntity<T>(Guid id, Guid accountId) where T : BaseRepository<T>, new()
        {
            var item = await m_DatabaseService.FirstOrDefaultAsync<T>(QueryFilter.Equal("Id", id), QueryFilter.Equal("AccountId", accountId));
            if (item == null) throw new ArgumentException("Account don't have access to item.");
        }

        public async Task<T> GetSavedEntityByAccount<T>(Guid id, Guid accountId) where T : BaseRepository<T>, new()
        {
            var item = await m_DatabaseService.FirstOrDefaultAsync<T>(QueryFilter.Equal("Id", id), QueryFilter.Equal("AccountId", accountId));

            if (item == null) throw new ArgumentException("Account don't have access to item.");

            return item;
        }

        public async Task<AccountRank> GetAccountRank(Guid accountId)
        {
            var account = await m_DatabaseService.FirstOrDefaultAsync<Account>(QueryFilter.Equal("Id", accountId));

            if (!GlobalSettings.AccountRanks.TryGetValue(account.RankId, out var rank)) return GlobalSettings.AccountRanks["free"];
            else return rank;
        }

        public void SetDatabase(IDatabaseService databaseService) => m_DatabaseService = databaseService;

        public async Task KataCheckUserHaveAccessToSavedEntity<T>(Guid id, Guid accountId) where T : class, new()
        {
            var item = await m_DatabaseService.FirstOrDefaultAsync<T>(new Query().Where("Id", id).Where("AccountId", accountId));
            if (item == null) throw new ArgumentException("Account don't have access to item.");
        }

    }

}
