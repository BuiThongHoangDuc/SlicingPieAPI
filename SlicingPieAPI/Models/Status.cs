using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Status
    {
        public Status()
        {
            StackHolders = new HashSet<StackHolder>();
            StackHolerDetails = new HashSet<StackHolerDetail>();
        }

        public string StatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<StackHolder> StackHolders { get; set; }
        public virtual ICollection<StackHolerDetail> StackHolerDetails { get; set; }
    }
}
