using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class AddAccountDto
    {
        public string AccountId { get; set; }
        public string NameAccount { get; set; }
        public string EmailAccount { get; set; }
        public decimal? PhoneAccount { get; set; }
        public string StatusId { get; set; }
        public int? RoleId { get; set; }
    }
}
