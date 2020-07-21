using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class StakeHolder
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
        public DateTime? DateTimeAdd { get; set; }

        public virtual Account Account { get; set; }
        public virtual Company Company { get; set; }
    }
}
