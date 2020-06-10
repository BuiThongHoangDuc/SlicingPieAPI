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
        public CompanyService(ICompanyRepository company)
        {
            _company = company;
        }

        public List<object> getListCompany(string name, int page_Index, int itemPerPage, string sort_Type, string field_Selected)
        {
            var companies = _company.Search(name);
            companies = _company.Paging(companies, page_Index, itemPerPage);
            companies = _company.Sort(companies, sort_Type);
            List<Object> list = _company.Filter(companies, field_Selected);
            return list;
        }
    }

    public interface ICompanyService
    {

        List<Object> getListCompany(string name, int page_Index, int itemPerPage, string sort_Type, string field_Selected);
    }
}
