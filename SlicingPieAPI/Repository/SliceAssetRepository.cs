using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class SliceAssetRepository : ISliceAssetRepository
    {
        private readonly SWD_SlicingPieContext _context;

        public SliceAssetRepository(SWD_SlicingPieContext context)
        {
            _context = context;
        }

        public double getTotalSliceSH(string companyID, string shID)
        {
            var totalSlice = _context.SliceAssets
                                            .Where(slice => slice.CompanyId == companyID && slice.AccountId == shID)
                                            .Select(slice => slice.AssetSlice).Sum();
            return totalSlice.Value;
        }
    }

    public interface ISliceAssetRepository
    {
        double getTotalSliceSH(string companyID, string shID);
    }
}
