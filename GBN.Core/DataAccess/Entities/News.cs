using System;
using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("News")]
    public class News : BaseRepository<News>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid AccountId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Link { get; set; }
        public Guid? ImageId { get; set; }
        public bool Visible { get; set; }
        public DateTime DatePublish { get; set; }
    }
}
