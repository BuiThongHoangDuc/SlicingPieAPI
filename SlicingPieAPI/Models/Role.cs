using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            StackHolders = new HashSet<StackHolder>();
        }

        public int RoleId { get; set; }
        public string NameRole { get; set; }

        public virtual ICollection<StackHolder> StackHolders { get; set; }
    }
}
