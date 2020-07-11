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
    public class TermProjectCompanyRepo : ITermProjectCompanyRepo
    {
        private readonly SWDSlicingPieContext _context;

        public TermProjectCompanyRepo(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TermDto>> GetListTermCompany(string companyID)
        {
            var listTerm = await _context.TermSlice
                                        .Where(term => term.CompanyId == companyID)
                                        .Select(term => new TermDto
                                        {
                                            TermId = term.TermId,
                                            TermName = term.TermName,
                                            TermTimeFrom = term.TermTimeFrom,
                                            TermTimeTo = term.TermTimeTo,
                                        }).ToListAsync();
            return listTerm;
        }

        public async Task<IEnumerable<ProjectDto>> getTermProjectCompany(int termID)
        {
            var termProject = await _context.ProjectDetails
                                                        .Where(pj => pj.TermId == termID && pj.Project.ProjectStatus == Status.ACTIVE)
                                                        .Select(pj => new ProjectDto {
                                                            ProjectId = pj.ProjectId,
                                                            ProjectName = pj.Project.ProjectName,
                                                        }).ToListAsync();
            return termProject;
        }
    }
    public interface ITermProjectCompanyRepo {
        Task<IEnumerable<ProjectDto>> getTermProjectCompany(int termID);
        Task<IEnumerable<TermDto>> GetListTermCompany(String companyID);
    }
}
