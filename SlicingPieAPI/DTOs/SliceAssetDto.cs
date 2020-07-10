using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class SliceAssetDto
    {
        public string AssetId { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }
        public DateTime TimeAsset { get; set; }
        public string ProjectId { get; set; }
        public String NamePerson { get; set; }
        public String TypeAsset { get; set; }
    }
}
