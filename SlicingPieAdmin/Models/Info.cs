using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Models
{
    public class Info
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("stakeHolderID")]
        public string StakeHolderID { get; set; }
        [JsonProperty("companyId")]
        public string CompanyId { get; set; }
        [JsonProperty("role")]
        public int Role { get; set; }
    }
}
