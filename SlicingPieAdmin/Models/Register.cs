using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Models
{
    public class Register
    {
        public String email { get; set; }
        public String password { get; set; }
        public bool returnSecureToken { get; set; }
    }
}
