using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class SliceAssetDetailStringDto
    {
        public string AssetId { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }
        public DateTime TimeAsset { get; set; }
        public int MultiplierInTime { get; set; }
        public string ProjectId { get; set; }
        public int? TypeAssetId { get; set; }
        public int? TermId { get; set; }
        public double? AssetSlice { get; set; }
        public double? SalaryGapInTime { get; set; }
        public double? CashPerSlice { get; set; }
    }
}
