using System;
using System.Collections.Generic;
using System.Text;
using UnivIntel.PostgreSQL.ORM.Core;

namespace UnivIntel.GBN.Core.DataAccess
{
    public class GBNBaseRepository<T> : BaseRepository<T> where T : class
    {
        public GBNBaseRepository()
        {
        }
    }
}
