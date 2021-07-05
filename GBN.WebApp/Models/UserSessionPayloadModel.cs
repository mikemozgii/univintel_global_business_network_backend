using System;

namespace UnivIntel.GBN.WebApp.Models
{

    public class UserSessionPayloadModel
    {

        public Guid AccountId { get; set; }

        public DateTime Expires { get; set; }

    }

}
