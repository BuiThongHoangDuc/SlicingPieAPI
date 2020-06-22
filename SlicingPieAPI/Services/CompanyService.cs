using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public async Task<string> updateCompany(string id, CompanyDetailDto company)
        {
            return await _company.UpdateCompany(id, company);
        }

        public Task<CompanyDetailDto> CreateCompany(CompanyDetailDto company)
        {
            string lastCompanyID = _company.getLastIDCompany();
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(lastCompanyID);

            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;
            int numberCompany = Int32.Parse(numberPart)+1;
            company.CompanyId = alphaPart + numberCompany;

            if (company.NonCashMultiplier == null) company.NonCashMultiplier = 2;
            if (company.CashMultiplier == null) company.CashMultiplier = 4;

            try
            {
                var companyInfo = _company.CreateCompany(company); 
                return companyInfo;
            }
            catch(DbUpdateException)
            {
                throw;
            }
        }
    }

    public interface ICompanyService
    {
        Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID);
        Task<SHLoadMainDto> getSHByCompany(string companyId, string shId);
        Task<CompanyDetailDto> getDetailCompany(string companyID);
        List<Object> getListCompany(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected);
        Task<string> updateCompany(string id, CompanyDetailDto company);
        Task<CompanyDetailDto> CreateCompany(CompanyDetailDto company);
    }
}
