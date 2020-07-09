using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class TypeAsset
    {
        public TypeAsset()
        {
            SliceAssets = new HashSet<SliceAsset>();
            TypeAssetCompanies = new HashSet<TypeAssetCompany>();
        }

        public int TypeAssetId { get; set; }
        public string NameAsset { get; set; }
        public string MultiplierType { get; set; }

        public virtual ICollection<SliceAsset> SliceAssets { get; set; }
        public virtual ICollection<TypeAssetCompany> TypeAssetCompanies { get; set; }
    }
}
