using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class SliceAssetRepository : ISliceAssetRepository
    {
        private readonly SWDSlicingPieContext _context;

        public SliceAssetRepository(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public async Task<bool> addSlice(SliceAssetDetailDto asset)
        {
            SliceAsset assetModel = new SliceAsset();
            assetModel.AssetId = asset.AssetId;
            assetModel.Quantity = asset.Quantity;
            assetModel.Description = asset.Description;
            assetModel.TimeAsset = asset.TimeAsset;
            assetModel.MultiplierInTime = asset.MultiplierInTime;
            assetModel.AccountId = asset.AccountId;
            assetModel.ProjectId = asset.ProjectId;
            assetModel.TypeAssetId = asset.TypeAssetId;
            assetModel.TermId = asset.TermId;
            assetModel.AssetStatus = Status.ACTIVE;
            assetModel.AssetSlice = asset.AssetSlice;
            assetModel.CompanyId = asset.CompanyId;
            


            _context.SliceAssets.Add(assetModel);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string> getLastIDAsset(String companyID)
        {
            var lastID = await _context.SliceAssets.Where(asset => asset.AssetStatus == Status.ACTIVE && asset.CompanyId == companyID).OrderByDescending(asset => asset.AssetId).Select(asset => asset.AssetId).FirstOrDefaultAsync();
            return lastID;
        }

        public double getTotalSliceSH(string companyID, string shID)
        {
            var totalSlice = _context.SliceAssets
                                            .Where(slice => slice.CompanyId == companyID && slice.AccountId == shID)
                                            .Select(slice => slice.AssetSlice).Sum();
            return totalSlice.Value;
        }

        public async Task<IEnumerable<TypeAssetCompany>> getType(string companyid, int typeid)
        {
            var type = await _context.TypeAssetCompanies.Where(tp => tp.TypeAssetId == typeid && tp.CompanyId == companyid).ToListAsync();
            return type;
        }
    }
    public interface ISliceAssetRepository
    {
        double getTotalSliceSH(string companyID, string shID);
        Task<bool> addSlice(SliceAssetDetailDto asset);
        Task<string> getLastIDAsset(String companyID);
        Task<IEnumerable<TypeAssetCompany>> getType(String companyid, int typeid);
    }
}
