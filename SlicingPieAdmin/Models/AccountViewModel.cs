using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Models
{
    public class AccountViewModel
    {
        [JsonProperty("AccountId")]
        public String AccountId { get; set; }
        [JsonProperty("NameAccount")]
        public String NameAccount { get; set; }
        [JsonProperty("EmailAccount")]
        public String EmailAccount { get; set; }
        [JsonProperty("PhoneAccount")]
        public decimal? PhoneAccount { get; set; }
        [JsonProperty("StatusId")]
        public String StatusId { get; set; }
        [JsonProperty("RoleId")]
        public int RoleId { get; set; }
    }
}
