using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Company
    {
        public Company()
        {
            Projects = new HashSet<Project>();
            SliceAssets = new HashSet<SliceAsset>();
            StakeHolders = new HashSet<StakeHolder>();
            TermSlice = new HashSet<TermSlouse>();
            TypeAssetCompanies = new HashSet<TypeAssetCompany>();
        }

        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyIcon { get; set; }
        public int NonCashMultiplier { get; set; }
        public int CashMultiplier { get; set; }
        public string Status { get; set; }
        public double? CashPerSlice { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<SliceAsset> SliceAssets { get; set; }
        public virtual ICollection<StakeHolder> StakeHolders { get; set; }
        public virtual ICollection<TermSlouse> TermSlice { get; set; }
        public virtual ICollection<TypeAssetCompany> TypeAssetCompanies { get; set; }
    }
}
