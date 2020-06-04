using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class StackHolder
    {
        public StackHolder()
        {
            SliceAssets = new HashSet<SliceAsset>();
            StackHolerDetails = new HashSet<StackHolerDetail>();
        }

        public string StackHolerId { get; set; }
        public string Shaccount { get; set; }
        public string Shemail { get; set; }
        public decimal? ShphoneNo { get; set; }
        public string StatusId { get; set; }
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<SliceAsset> SliceAssets { get; set; }
        public virtual ICollection<StackHolerDetail> StackHolerDetails { get; set; }
    }
}
