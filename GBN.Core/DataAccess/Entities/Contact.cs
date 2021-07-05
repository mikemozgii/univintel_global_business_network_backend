using System;
using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;

namespace UnivIntel.GBN.Core.DataAccess.Entities
{
    [PgsDataTable("Contacts")]
    public class Contact : BaseRepository<Contact>
    {
        [PgsPK]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public Guid CityId { get; set; }
        public Guid AccountId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public Guid CompanyId { get; set; }
        public bool Visible { get; set; }
        public bool VisibleEmail { get; set; }
        public Guid? ImageId { get; set; }
        public bool VisiblePhone { get; set; }
    }

}
