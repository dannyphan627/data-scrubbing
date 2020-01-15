using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class InsuranceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
