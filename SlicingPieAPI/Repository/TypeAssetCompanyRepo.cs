using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class TypeAssetCompanyRepo : ITypeAssetCompanyRepo
    {
        private readonly SWDSlicingPieContext _context;

        public TypeAssetCompanyRepo(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public async Task CreateAssetCompany(int typeAssetID, string companyID)
        {
            TypeAssetCompany type = new TypeAssetCompany();
            type.CompanyId = companyID;
            type.TypeAssetId = typeAssetID;
            

            _context.TypeAssetCompanies.Add(type);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<IEnumerable<string>> GetListTypeAssetByCompanyID(string companyID)
        {
            var listType = await _context.TypeAssetCompanies
                                            .Where(type => type.CompanyId == companyID)
                                            .Select(type => type.TypeAsset.NameAsset).ToListAsync();
            return listType;
        }
    }
    public interface ITypeAssetCompanyRepo
    {
        Task CreateAssetCompany(int typeAssetID, String companyID);
        Task<IEnumerable<String>> GetListTypeAssetByCompanyID(String companyID);

    }
}
