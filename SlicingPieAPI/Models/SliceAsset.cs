using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class SliceAsset
    {
        public string AssetId { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }
        public DateTime TimeAsset { get; set; }
        public int MultiplierInTime { get; set; }
        public string AccountId { get; set; }
        public string ProjectId { get; set; }
        public int? TypeAssetId { get; set; }
        public int? TermId { get; set; }
        public string AssetStatus { get; set; }
        public double? AssetSlice { get; set; }
        public string CompanyId { get; set; }
        public double? SalaryGapInTime { get; set; }
        public double? CashPerSlice { get; set; }

        public virtual Account Account { get; set; }
        public virtual Company Company { get; set; }
        public virtual Project Project { get; set; }
        public virtual TermSlouse Term { get; set; }
        public virtual TypeAsset TypeAsset { get; set; }
    }
}
