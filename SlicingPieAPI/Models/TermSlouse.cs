using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class TermSlouse
    {
        public TermSlouse()
        {
            ProjectDetails = new HashSet<ProjectDetail>();
            SliceAssets = new HashSet<SliceAsset>();
        }

        public int TermId { get; set; }
        public DateTime TermTimeFrom { get; set; }
        public DateTime TermTimeTo { get; set; }
        public string CompanyId { get; set; }
        public string TermName { get; set; }
        public string TermStatus { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
        public virtual ICollection<SliceAsset> SliceAssets { get; set; }
    }
}
