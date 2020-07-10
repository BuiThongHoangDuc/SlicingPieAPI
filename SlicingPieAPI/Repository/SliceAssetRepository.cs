using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            assetModel.CashPerSlice = asset.CashPerSlice;
            assetModel.SalaryGapInTime = asset.SalaryGapInTime;


            _context.SliceAssets.Add(assetModel);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.InnerException.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsset(string asssetID)
        {
            var asset = await _context.SliceAssets.FindAsync(asssetID);
            if (asset == null)
            {
                return false;
            }
            try
            {
                asset.AssetStatus = Status.INACTIVE;
                _context.Entry(asset).State = EntityState.Modified;
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

        public async Task<IEnumerable<SliceAssetDto>> GetListSlice(string companyID)
        {
            var ListContribution = await _context.SliceAssets
                                           .Where(asset => asset.AssetStatus == Status.ACTIVE && asset.CompanyId == companyID)
                                           .Select(asset => new SliceAssetDto
                                           {
                                               AssetId = asset.AssetId,
                                               NamePerson = asset.Account.StakeHolders
                                                               .Where(sh => sh.CompanyId == companyID && sh.AccountId == asset.AccountId)
                                                               .Select(sh => sh.ShnameForCompany).FirstOrDefault(),
                                               Quantity = asset.Quantity,
                                               Description = asset.Description,
                                               ProjectId = asset.ProjectId,
                                               TimeAsset = asset.TimeAsset,
                                               TypeAsset = asset.TypeAsset.NameAsset,
                                           }).ToListAsync();
            return ListContribution;
        }

        public async Task<IEnumerable<SliceAssetDto>> GetListSliceSH(string companyID, string SHID)
        {
            var ListContribution = await _context.SliceAssets
                                           .Where(asset => asset.AssetStatus == Status.ACTIVE && asset.CompanyId == companyID && asset.AccountId == SHID)
                                           .Select(asset => new SliceAssetDto
                                           {
                                               AssetId = asset.AssetId,
                                               NamePerson = asset.Account.StakeHolders
                                                               .Where(sh => sh.CompanyId == companyID && sh.AccountId == asset.AccountId)
                                                               .Select(sh => sh.ShnameForCompany).FirstOrDefault(),
                                               Quantity = asset.Quantity,
                                               Description = asset.Description,
                                               ProjectId = asset.ProjectId,
                                               TimeAsset = asset.TimeAsset,
                                               TypeAsset = asset.TypeAsset.NameAsset,
                                           }).ToListAsync();
            return ListContribution;
        }

        public async Task<SliceAssetDetailStringDto> GetSliceByID(string assetID)
        {
            var Contribution = await _context.SliceAssets
                                            .Where(asset => asset.AssetStatus == Status.ACTIVE && asset.AssetId == assetID)
                                            .Select(asset => new SliceAssetDetailStringDto
                                            {
                                                AssetId = asset.AssetId,                                               
                                                AssetSlice = asset.AssetSlice,
                                                Description = asset.Description,
                                                MultiplierInTime = asset.MultiplierInTime,
                                                ProjectId = asset.ProjectId,
                                                Quantity = asset.Quantity,
                                                TermId = asset.TermId,
                                                TimeAsset = asset.TimeAsset,
                                                TypeAssetId = asset.TypeAssetId, 
                                            }).FirstOrDefaultAsync();
            return Contribution;
        }

        public double getTotalSliceSH(string companyID, string shID)
        {
            var totalSlice = _context.SliceAssets
                                            .Where(slice => slice.CompanyId == companyID && slice.AccountId == shID)
                                            .Select(slice => slice.AssetSlice).Sum();
            return totalSlice.Value;
        }

        public async Task UpdateAsset(string assetID, SliceAssetDetailStringDto asset)
        {
            SliceAsset assetModel = await _context.SliceAssets.FindAsync(assetID);
            if (assetModel == null) return;
            assetModel.Quantity = asset.Quantity;
            assetModel.ProjectId = asset.ProjectId;
            assetModel.Description = asset.Description;
            assetModel.TimeAsset = asset.TimeAsset;
            assetModel.ProjectId = asset.ProjectId;
            assetModel.TypeAssetId = asset.TypeAssetId;
            assetModel.TermId = asset.TermId;
            assetModel.AssetSlice = asset.AssetSlice;

            _context.Entry(assetModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               throw;
            }

        }
    }
    public interface ISliceAssetRepository
    {
        double getTotalSliceSH(string companyID, string shID);
        Task<bool> addSlice(SliceAssetDetailDto asset);
        Task<string> getLastIDAsset(String companyID);
        Task<IEnumerable<SliceAssetDto>> GetListSlice(String companyID);
        Task<IEnumerable<SliceAssetDto>> GetListSliceSH(String companyID, String SHID);
        Task<SliceAssetDetailStringDto> GetSliceByID(String assetID);
        Task UpdateAsset(String assetID, SliceAssetDetailStringDto asset);
        Task<bool> DeleteAsset(String asssetID);
    }
}
