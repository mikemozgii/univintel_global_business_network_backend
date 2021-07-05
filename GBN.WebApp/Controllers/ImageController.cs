using System;
using System.IO;
using System.Threading.Tasks;
using GBN.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Models;
using UnivIntel.PostgreSQL.ORM.Core.Uuid;

namespace UnivIntel.GBN.WebApp.Controllers
{

    [Route("api/1/images")]
    [ApiController]
    public class ImageController : SessionController
    {
        private readonly IQueryExecutionFactory m_QueryExecutionFactory;

        public ImageController(IQueryExecutionFactory queryExecutionFactory) => m_QueryExecutionFactory = queryExecutionFactory ?? throw new ArgumentNullException(nameof(queryExecutionFactory));
        
        [Route("add")]
        [HttpPost]
        public async Task<string> Upload([FromForm]FormFileModel formFile)
        {
            if (formFile != null && formFile.file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await formFile.file.CopyToAsync(ms);
                    var id = !formFile.Id.HasValue ? new PKuuidSvc().GenereateTradingGuid() : formFile.Id.Value;
                    await GetDatabase(m_QueryExecutionFactory).SaveImage(id, ms.ToArray(), formFile.Type, Session.Id, isUpdate: formFile.Id.HasValue);
                    return id.ToString();
                }
            }
            return "";
        }
        [Route("avatar")]
        [HttpGet]
        public async Task<ActionResult> Download([FromQuery]Guid id)
        {
            var bytes = await GetDatabase(m_QueryExecutionFactory).GetImage(id);
            return File(bytes, "application/octet-stream");
        }
    }
}