using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GBN.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlKata;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Models;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.GBN.WebApp.Controllers
{
    [Route("api/1/files")]
    [ApiController]
    public class FilesController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;
        private readonly ISessionService m_SessionService;

        public FilesController(IQueryExecutionFactory queryExecutionFactory, ISessionService sessionService)
        {
            m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
            m_SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        [Route("upload")]
        [HttpPost]
        public async Task<string> Upload([FromForm]FormFileModel formFile)
        {
            if (formFile != null && formFile.file.Length > 0)
            {
                var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);
                var fileModel = new GBN.Core.DataAccess.Entities.File
                {
                    Uploaded = DateTime.UtcNow,
                    Name = formFile.file.FileName,
                    Type = formFile.file.ContentType,
                    AccountId = Session.Id,
                    Tag = formFile.Tag
                };
                if (formFile.CompanyId.HasValue)
                {
                    await m_SessionService.CheckUserHaveAccessToCompany(formFile.CompanyId.Value, Session.Id);
                    fileModel.CompanyId = formFile.CompanyId.Value;
                }
                using (var ms = new MemoryStream())
                {
                    await formFile.file.CopyToAsync(ms);
                    fileModel.Size = ms.Length;

                    if (!formFile.Id.HasValue) {
                        await database.InsertAsync(fileModel);
                    } else {
                        //TODO: check for access to edited item
                        fileModel.Id = formFile.Id.Value;
                        await database.UpdateAsync(fileModel, QueryFilter.Equal("Id", fileModel.Id));
                    }
                    
                    await database.SaveFileData(fileModel.Id, ms.ToArray());
                    return fileModel.Id.ToString();
                }
            }
            return "";
        }

        [Route("download")]
        [HttpGet]
        public async Task<ActionResult> Download([FromQuery]Guid id)
        {
            var database = GetDatabaseWithSession(m_QueryExecutionFactory, m_SessionService);

            var query = new Query("Files").Where("Id", "=", id);

            var result = (await database.WhereAsync<GBN.Core.DataAccess.Entities.File>(query)).FirstOrDefault();

            if (result == null) return BadRequest(404);

            var bytes = await GetDatabase(m_QueryExecutionFactory).GetFile(id);

            return File(bytes, result.Type, result.Name);
        }
    }
}