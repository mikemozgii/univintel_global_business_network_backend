using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnivIntel.GBN.Core.Models
{
    public class CompanyEditMdl
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
