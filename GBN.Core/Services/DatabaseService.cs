using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Univintel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Services;
using SqlKata;
using SqlKata.Compilers;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using System.Reflection;
using System.Threading;
using SqlKata.Execution;

namespace UnivIntel.GBN.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly QueryExecutionSvc Context;

        public DatabaseService(string connectionString, string applicationName)
        {
            Context = new QueryExecutionSvc(connectionString, applicationName);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(params KeyValuePair<string, object>[] filters) where T : BaseRepository<T>, new()
        {
            var result = await Context.SELECTbyColumnValueAsync<T>(filters);

            return result.HasResults ? result.Results : Enumerable.Empty<T>();
        }

        public async Task<IEnumerable<T>> FilterAsync<T>(QueryFilter filter) where T : BaseRepository<T>, new()
        {
            var result = await Context.SELECTAsync<T>(filter);

            return result.HasResults ? result.Results : Enumerable.Empty<T>();
        }



        public async Task<IEnumerable<T>> AllAsync<T>() where T : BaseRepository<T>, new()
        {
            var result = await Context.SELECTALLAsync<T>();

            return result.HasResults ? result.Results : Enumerable.Empty<T>();
        }

        public async Task<T> InsertAsync<T>(T model) where T : BaseRepository<T>, new()
        {
            var result = await Context.INSERTAsync(model);
            return result.HasResults ? result.Results.First() : null;
        }

        public async Task<T> UpdateAsync<T>(T model, params QueryFilter[] filters) where T : BaseRepository<T>, new()
        {
            var result = await Context.UPDATEAsync(model, QueryFilter.And(filters));
            return result.HasResults ? result.Results.First() : null;
        }

        public async Task DeleteAsync<T>(params QueryFilter[] filters) where T : BaseRepository<T>, new()
        {
            await Context.DELETEAsync<T>(QueryFilter.And(filters));
        }

        public async Task<IEnumerable<T>> RawSelectAsync<T>(string sql) where T : BaseRepository<T>, new()
        {
            var result = await Context.ExecuteResultQueryAsync<T>(new SelectQueryGeneric<T>(sql).Request);
            return result.Results;
        }


        public void SetApplicationName(string applicationName) => Context.ApplicationName = applicationName;

        public async Task<T> FirstOrDefaultAsync<T>(params QueryFilter[] filter) where T : BaseRepository<T>, new()
        {
            var result = await Context.SELECTAsync<T>(QueryFilter.And(filter));

            return result.HasResults ? result.Results.FirstOrDefault() : null;
        }

        public IEnumerable<T> Filter<T>(params QueryFilter[] filters) where T : BaseRepository<T>, new()
        {
            var result = Context.SELECT<T>(QueryFilter.And(filters));

            return result.HasResults ? result.Results : Enumerable.Empty<T>();
        }

        public async Task<IEnumerable<T>> WhereAsync<T>(params QueryFilter[] filters) where T : BaseRepository<T>, new()
        {
            var result = await Context.SELECTAsync<T>(QueryFilter.And(filters));

            return result.HasResults ? result.Results : Enumerable.Empty<T>();
        }

        public async Task SaveImage(Guid Id, byte[] data, string type, Guid accountId, bool isUpdate = false)
        {
            using (var connection = new NpgsqlConnection(Context.ConnStr))
            {
                string sql = "";
                if (isUpdate)
                {
                    sql = "update \"Images\" SET  \"Data\"=@Image, \"Type\"=@Type WHERE \"Id\"=@Id";
                }
                else
                {
                    sql = "insert into \"Images\" (\"Id\", \"AccountId\", \"Data\", \"Type\") VALUES(@Id, @AccountId, @Image, @Type)";
                }

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    NpgsqlParameter param = command.CreateParameter();
                    param.ParameterName = "@Image";
                    param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                    param.Value = data;
                    command.Parameters.Add(param);

                    NpgsqlParameter paramId = command.CreateParameter();
                    paramId.ParameterName = "@Id";
                    paramId.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    paramId.Value = Id;
                    command.Parameters.Add(paramId);

                    NpgsqlParameter paramType = command.CreateParameter();
                    paramType.ParameterName = "@Type";
                    paramType.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text;
                    paramType.Value = string.IsNullOrEmpty(type) ? "avatar" : type;
                    command.Parameters.Add(paramType);

                    NpgsqlParameter paramAccountId = command.CreateParameter();
                    paramAccountId.ParameterName = "@AccountId";
                    paramAccountId.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    paramAccountId.Value = accountId;
                    command.Parameters.Add(paramAccountId);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task SaveFileData(Guid Id, byte[] data)
        {
            using (var connection = new NpgsqlConnection(Context.ConnStr))
            {
                string sql = "update \"Files\" SET \"Data\" = @Data WHERE \"Id\" = @Id";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    NpgsqlParameter param = command.CreateParameter();
                    param.ParameterName = "@Data";
                    param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                    param.Value = data;
                    command.Parameters.Add(param);

                    NpgsqlParameter paramId = command.CreateParameter();
                    paramId.ParameterName = "@Id";
                    paramId.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    paramId.Value = Id;
                    command.Parameters.Add(paramId);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<byte[]> GetImage(Guid Id)
        {
            using (var connection = new NpgsqlConnection(Context.ConnStr))
            {
                string sql = "select \"Data\" from \"Images\" where \"Id\" = @Id";// and \"AccountId\" = @AccountId
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    NpgsqlParameter paramId = command.CreateParameter();
                    paramId.ParameterName = "@Id";
                    paramId.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    paramId.Value = Id;
                    command.Parameters.Add(paramId);

                    byte[] productImageByte = null;
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        productImageByte = (byte[])reader[0];
                    }
                    reader.Close();

                    return productImageByte;
                }
            }
        }

        public async Task<byte[]> GetFile(Guid Id)
        {
            using (var connection = new NpgsqlConnection(Context.ConnStr))
            {
                string sql = "select \"Data\" from \"Files\" where \"Id\" = @Id";// and \"AccountId\" = @AccountId
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    NpgsqlParameter paramId = command.CreateParameter();
                    paramId.ParameterName = "@Id";
                    paramId.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
                    paramId.Value = Id;
                    command.Parameters.Add(paramId);

                    byte[] productImageByte = null;
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        productImageByte = (byte[])reader[0];
                    }
                    reader.Close();

                    return productImageByte;
                }
            }
        }

        public async Task SaveLocationGeometry(Guid Id, double latitude, double longitude)
        {
            var stringLatitude = latitude.ToString().Replace(",", ".");
            var stringLongitude = longitude.ToString().Replace(",", ".");
            await RawSelectAsync<Location>("UPDATE \"Locations\" SET \"Point\" = ST_GeomFromText('POINT(" + stringLatitude + " " + stringLongitude + ")', 4326) WHERE \"Id\" = '" + Id.ToString() + "'");
        }

        public async Task<(double, double)> GetLocationGeometry(Guid id)
        {
            var result = await RawSelectAsync<LocationPoint>("SELECT ST_AsText(\"Point\") AS \"Point\" FROM \"Locations\" WHERE \"Id\" = '" + id.ToString() + "'");
            var entity = result.FirstOrDefault();
            if (entity == null) return (0, 0);

            var parts = entity.Point.Replace("POINT(", "").Replace(")", "").Split(" ");
            return (Convert.ToDouble(parts[0].Replace(".", ",")), Convert.ToDouble(parts[1].Replace(".", ",")));
        }

        public async Task<IEnumerable<T>> WhereAsync<T>(Query query) where T : BaseRepository<T>, new()
        {
            var sqlResult = new PostgresCompiler().Compile(query);

            var result = await Context.ExecuteResultQueryAsync<T>(new SelectQueryGeneric<T>(sqlResult.ToString()).Request);
            return result.Results;
        }

        public async Task<IEnumerable<T>> KataQueryAsync<T>(Query query) where T : class
        {
            var result = await Context.SelectAsync<T>(query);
            return result.Results;
        }

        public async Task<T> FirstOrDefaultAsync<T>(Query query) where T : class
        {
            var result = await Context.SelectAsync<T>(query);
            return result.Results.FirstOrDefault();
        }

        public async Task<T> AddAsync<T>(T item, List<string> exceptedFields = null) where T : class
        {
            var result = await Context.AddAsync(item, exceptedFields);
            return result.Results?.FirstOrDefault();
        }

        public async Task<T> UpdateAsync<T>(T model, Query query, List<string> exceptedFields = null) where T : class
        {
            var result = await Context.UpdateAsync(model, query, exceptedFields);
            return result.Results?.FirstOrDefault();
        }

    }

}
