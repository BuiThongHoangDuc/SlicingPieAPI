using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class UserLoginDto
    {
        public string StackHolderID { get; set; }
        public int? RoleID { get; set; }
        public string StatusID { get; set; }
        public string CompanyID { get; set; }
    }
}
