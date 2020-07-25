using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class OverViewCompany
    {
        public string CompanyImage { get; set; }
        public string CompanyName { get; set; }
        public double? CashPerSlice { get; set; }
        public int TotalTerm { get; set; }
        public double TotalSlice { get; set; }
        public int TotalStakeholder { get; set; }
    }
}
