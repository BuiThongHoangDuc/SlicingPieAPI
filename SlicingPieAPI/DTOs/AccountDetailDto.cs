﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class AccountDetailDto
    {
        public string AccountId { get; set; }
        public string NameAccount { get; set; }
        public string EmailAccount { get; set; }
        public decimal? PhoneAccount { get; set; }
    }
}
