using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class TermOfCompany
    {
        public TermOfCompany()
        {
            Assets = new HashSet<Asset>();
            ProjectDetails = new HashSet<ProjectDetail>();
        }

        public int TermId { get; set; }
        public DateTime TermTimeFrom { get; set; }
        public DateTime TermTimeTo { get; set; }
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
    }
}
