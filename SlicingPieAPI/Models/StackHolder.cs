using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class StackHolder
    {
        public StackHolder()
        {
            Assets = new HashSet<Asset>();
            StackHolerDetails = new HashSet<StackHolerDetail>();
        }

        public string StackHolerId { get; set; }
        public string Shaccount { get; set; }
        public string Shpassword { get; set; }
        public string Shemail { get; set; }
        public decimal? ShphoneNo { get; set; }
        public string StatusId { get; set; }
        public int? RoleId { get; set; }
        public string Shname { get; set; }

        public virtual Role Role { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<StackHolerDetail> StackHolerDetails { get; set; }
    }
}
