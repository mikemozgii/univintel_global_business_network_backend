using Microsoft.AspNetCore.Http;
using System;

namespace UnivIntel.GBN.WebApp.Models
{
    public class FormFileModel
    {
        public IFormFile file { get; set; }
        public Guid? CompanyId { get; set; }
        public string Tag { get; set; }
        public string Type { get; set; }
        public Guid? Id { get; set; }
    }
}
