using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.DTOs
{
    public class SHLoadMainDto
    {
        public string SHID { get; set; }
        public string SHName { get; set; }
        public string SHJob { get; set; }
        public string SHImage { get; set; }
        public string CompanyID { get; set; }
        public double? SliceAssets { get; set; }
    }
}
