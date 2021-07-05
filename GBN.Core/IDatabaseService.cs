using SqlKata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace Univintel.GBN.Core
{
    public interface IDatabaseService
    {

        void SetApplicationName(string applicationName);

        Task<T> FirstOrDefaultAsync<T>(params QueryFilter[] filter) where T : BaseRepository<T>, new();

        Task<IEnumerable<T>> SelectAsync<T>(params KeyValuePair<string, object>[] filters) where T : BaseRepository<T>, new();

        Task<IEnumerable<T>> WhereAsync<T>(params QueryFilter[] filters) where T : BaseRepository<T>, new();

        Task<IEnumerable<T>> FilterAsync<T>(QueryFilter filter) where T : BaseRepository<T>, new();

        IEnumerable<T> Filter<T>(params QueryFilter[] filters) where T : BaseRepository<T>, new();

        Task<IEnumerable<T>> AllAsync<T>() where T : BaseRepository<T>, new();

        Task<T> InsertAsync<T>(T model) where T : BaseRepository<T>, new();

        Task<T> UpdateAsync<T>(T model, params QueryFilter[] filters) where T : BaseRepository<T>, new();

        Task DeleteAsync<T>(params QueryFilter[] filters) where T : BaseRepository<T>, new();

        Task<IEnumerable<T>> RawSelectAsync<T>(string sql) where T : BaseRepository<T>, new();

        Task SaveImage(Guid Id, byte[] data, string type, Guid accountId, bool isUpdate = false);

        Task<byte[]> GetImage(Guid Id);

        Task SaveLocationGeometry(Guid Id, double latitude, double langitude);

        Task<(double, double)> GetLocationGeometry(Guid Id);

        Task<IEnumerable<T>> WhereAsync<T>(Query query) where T : BaseRepository<T>, new();

        Task SaveFileData(Guid Id, byte[] data);

        Task<byte[]> GetFile(Guid Id);

        Task<T> AddAsync<T>(T item, List<string> exceptedFields = null) where T : class;

        Task<IEnumerable<T>> KataQueryAsync<T>(Query query) where T : class;

        Task<T> FirstOrDefaultAsync<T>(Query query) where T : class;

        Task<T> UpdateAsync<T>(T model, Query query, List<string> exceptedFields = null) where T : class;

    }
}
