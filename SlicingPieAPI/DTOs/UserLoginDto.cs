using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class UserLoginDto
    {
        public string AccountID { get; set; }
        public string NameAccount { get; set; }
        public int? RoleID { get; set; }
    }
}
