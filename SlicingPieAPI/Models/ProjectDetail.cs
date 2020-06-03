using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class ProjectDetail
    {
        public int TermId { get; set; }
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual TermOfCompany Term { get; set; }
    }
}
