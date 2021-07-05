using System;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Files")]
    public class File : GBNBaseRepository<File>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime Uploaded { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CompanyId { get; set; }
        public string Tag { get; set; }
    }
}
