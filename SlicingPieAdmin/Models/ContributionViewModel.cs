using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Models
{
    public class ContributionViewModel
    {
        [JsonProperty("assetId")]
        public String AssetId { get; set; }
        [JsonProperty("quantity")]
        public double Quantity { get; set; }
        [JsonProperty("description")]
        public String Description { get; set; }
        [JsonProperty("timeAsset")]
        public DateTime TimeAsset { get; set; }
        [JsonProperty("project")]
        public String Project { get; set; }
        [JsonProperty("namePerson")]
        public String NamePerson { get; set; }
        [JsonProperty("typeAsset")]
        public String TypeAsset { get; set; }
    }
}
