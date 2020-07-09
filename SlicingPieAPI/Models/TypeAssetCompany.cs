using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class TypeAssetCompany
    {
        public int TypeAssetId { get; set; }
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual TypeAsset TypeAsset { get; set; }
    }
}
