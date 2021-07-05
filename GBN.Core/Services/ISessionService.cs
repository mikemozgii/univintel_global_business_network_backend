using System;
using System.Threading.Tasks;
using Univintel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.PostgreSQL.ORM.Core;

namespace UnivIntel.GBN.Core.Services
{
    public interface ISessionService
    {
        void SetDatabase(IDatabaseService databaseService);

        Task CheckUserHaveAccessToCompany(Guid companyId, Guid accountId);

        Task CheckUserHaveAccessToSavedEntity<T>(Guid id, Guid accountId) where T: BaseRepository<T>, new();
        
        Task KataCheckUserHaveAccessToSavedEntity<T>(Guid id, Guid accountId) where T : class, new();

        Task<T> GetSavedEntityByAccount<T>(Guid id, Guid accountId) where T : BaseRepository<T>, new();

        Task<AccountRank> GetAccountRank(Guid accountId);

    }
}