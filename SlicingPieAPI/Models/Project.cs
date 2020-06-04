using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Project
    {
        public Project()
        {
            ProjectDetails = new HashSet<ProjectDetail>();
            SliceAssets = new HashSet<SliceAsset>();
        }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
        public virtual ICollection<SliceAsset> SliceAssets { get; set; }
    }
}
