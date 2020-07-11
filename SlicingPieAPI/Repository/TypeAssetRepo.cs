using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class TypeAssetRepo : ITypeAssetRepo
    {
        private readonly SWDSlicingPieContext _context;

        public TypeAssetRepo(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public IQueryable<int> getListTypeAsset()
        {
            var listId = _context.TypeAssets.Select(type => type.TypeAssetId).Take(5);
            return listId;
        }

        public async Task<string> GetMultiplierById(int typeID)
        {
            var multiplier = await _context.TypeAssets
                                            .Where(tp => tp.TypeAssetId == typeID)
                                            .Select(ty => ty.MultiplierType)
                                            .FirstOrDefaultAsync();
            return multiplier;
        }
    }
    public interface ITypeAssetRepo
    {
        IQueryable<int> getListTypeAsset();
        Task<String> GetMultiplierById(int typeID);
    }
}
