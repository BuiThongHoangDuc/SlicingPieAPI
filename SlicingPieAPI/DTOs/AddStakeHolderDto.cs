using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class AddStakeHolderDto
    {
        public string AccountId { get; set; }
        public string CompanyId { get; set; }
        public double? ShmarketSalary { get; set; }
        public double? Shsalary { get; set; }
        public string Shjob { get; set; }
        public string ShnameForCompany { get; set; }
        public string Shimage { get; set; }
        public string Shstatus { get; set; }
        public int? Shrole { get; set; }
    }
}
