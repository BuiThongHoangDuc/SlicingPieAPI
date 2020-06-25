using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Models
{
    public class Company
    {
        [JsonProperty("CompanyId")]
        public String CompanyId { get; set; }
        [JsonProperty("CompanyName")]
        public String CompanyName { get; set; }
        [JsonProperty("CompanyIcon")]
        public String? CompanyIcon { get; set; }
        [JsonProperty("NonCashMultiplier")]
        public int NonCashMultiplier { get; set; }
        [JsonProperty("CashMultiplier")]
        public int CashMultiplier { get; set; }
    }
}
