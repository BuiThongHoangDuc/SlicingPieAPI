using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Account
    {
        public Account()
        {
            SliceAssets = new HashSet<SliceAsset>();
            StakeHolders = new HashSet<StakeHolder>();
        }

        public string AccountId { get; set; }
        public string NameAccount { get; set; }
        public string EmailAccount { get; set; }
        public decimal? PhoneAccount { get; set; }
        public string StatusId { get; set; }
        public int? RoleId { get; set; }

        public virtual ICollection<SliceAsset> SliceAssets { get; set; }
        public virtual ICollection<StakeHolder> StakeHolders { get; set; }
    }
}
