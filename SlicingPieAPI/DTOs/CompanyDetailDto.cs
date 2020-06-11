using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class CompanyDetailDto
    {
        public string CompanyName { get; set; }
        public string ComapnyIcon { get; set; }
        public int? NonCashMultiplier { get; set; }
        public int? CashMultiplier { get; set; }
    }
}
