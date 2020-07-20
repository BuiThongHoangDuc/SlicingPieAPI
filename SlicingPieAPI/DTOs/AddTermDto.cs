using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class AddTermDto
    {
        public DateTime TermTimeFrom { get; set; }
        public DateTime TermTimeTo { get; set; }
        public string CompanyId { get; set; }
        public string TermName { get; set; }

    }
}
