using SlicingPieAPI.DTOs;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _company;
        private readonly IStakeHolderRepository _stakeHolder;
        private readonly ISliceAssetRepository _sliceRepository;
        public CompanyService(ICompanyRepository company, IStakeHolderRepository stakeHolder, ISliceAssetRepository sliceRepository)
        {
            _company = company;
            _stakeHolder = stakeHolder;
            _sliceRepository = sliceRepository;
        }

        public List<object> getListCompany(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected)
        {
            var companies = _company.Search(name);
            companies = _company.Sort(companies, sort_Type);
            companies = _company.Paging(companies, page_Index, itemPerPage);
            List<Object> list = _company.Filter(companies, field_Selected);
            return list;
        }

        public async Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID)
        {
            var info = await _stakeHolder.getListShByCompany(companyID);
            return info;
        }

        public async Task<CompanyDetailDto> getDetailCompany(string companyID) {
            var companyDetail = await _company.getDetailCompany(companyID);
            return companyDetail;
        }

        public async Task<SHLoadMainDto> getSHByCompany(string companyId, string shId)
        {
            return await _stakeHolder.getShByIDCompany(companyId, shId);
        }
    }

    public interface ICompanyService
    {
        Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID);
        Task<SHLoadMainDto> getSHByCompany(string companyId, string shId);
        Task<CompanyDetailDto> getDetailCompany(string companyID);
        List<Object> getListCompany(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected);
    }
}
