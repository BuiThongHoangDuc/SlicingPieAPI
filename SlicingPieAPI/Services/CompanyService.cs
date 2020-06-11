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
        public CompanyService(ICompanyRepository company, IStakeHolderRepository stakeHolder)
        {
            _company = company;
            _stakeHolder = stakeHolder;
        }

        public List<object> getListCompany(string name, int page_Index, int itemPerPage, string sort_Type, string field_Selected)
        {
            var companies = _company.Search(name);
            companies = _company.Paging(companies, page_Index, itemPerPage);
            companies = _company.Sort(companies, sort_Type);
            List<Object> list = _company.Filter(companies, field_Selected);
            return list;
        }

        public async Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID)
        {
            var info = await _stakeHolder.getListShByCompany(companyID);
            return info;
        }

    }

    public interface ICompanyService
    {
        Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID);
        List<Object> getListCompany(string name, int page_Index, int itemPerPage, string sort_Type, string field_Selected);
    }
}
