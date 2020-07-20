using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Models
{
    public class StakeHolderInCompany
    {
        [JsonProperty("shid")]
        public String ShId { get; set; }
        [JsonProperty("shName")]
        public String ShName { get; set; }
        [JsonProperty("shJob")]
        public String ShJob { get; set; }
        [JsonProperty("shImage")]
        public String ShImage { get; set; }
        [JsonProperty("companyId")]
        public String CompanyID { get; set; }
        [JsonProperty("sliceAssets")]
        public double SliceAssets { get; set; }
    }
}
