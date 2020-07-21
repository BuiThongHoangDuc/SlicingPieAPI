using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class TermDto
    {
        public int TermId { get; set; }
        public DateTime TermTimeFrom { get; set; }
        public DateTime TermTimeTo { get; set; }
        public string TermName { get; set; }
        public double? TermSliceTotal { get; set; }
    }
}
