using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Project
    {
        public Project()
        {
            Assets = new HashSet<Asset>();
            ProjectDetails = new HashSet<ProjectDetail>();
        }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
    }
}
