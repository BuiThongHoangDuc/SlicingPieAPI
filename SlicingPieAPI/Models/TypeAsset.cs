using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class TypeAsset
    {
        public TypeAsset()
        {
            Assets = new HashSet<Asset>();
        }

        public int TypeAssetId { get; set; }
        public string NameAsset { get; set; }
        public int Multiplier { get; set; }
        public string MultiplierType { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
