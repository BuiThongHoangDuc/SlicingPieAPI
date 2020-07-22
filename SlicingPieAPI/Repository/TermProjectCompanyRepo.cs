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
    public class TermProjectCompanyRepo : ITermProjectCompanyRepo
    {
        private readonly SWDSlicingPieContext _context;

        public TermProjectCompanyRepo(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProjectToTerm(int termID, string projectID)
        {
            ProjectDetail pjdt = new ProjectDetail();
            pjdt.TermId = termID;
            pjdt.ProjectId = projectID;
            _context.ProjectDetails.Add(pjdt);
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

        public async Task<bool> addTermCompany(AddTermDto term)
        {
            TermSlouse termModel = new TermSlouse();
            termModel.TermName = term.TermName;
            termModel.TermTimeFrom = term.TermTimeFrom;
            termModel.TermTimeTo = term.TermTimeTo;
            termModel.CompanyId = term.CompanyId;
            termModel.TermStatus = "1";


            _context.TermSlice.Add(termModel);
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

        public async Task<DateTime> GetLatestTimeto(string companyID)
        {
            var listTerm = await _context.TermSlice
                                        .Where(term => term.CompanyId == companyID)
                                        .Select(term =>

                                          term.TermTimeTo
                                        ).LastOrDefaultAsync();
            return listTerm;
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
                                            TermSliceTotal = term.SliceAssets.Where(termSlice => term.TermId == termSlice.TermId).Select(termSlice => termSlice.AssetSlice).Sum() ?? 0,
                                            TermStatus = term.TermStatus,
                                        }).OrderByDescending(term => term.TermTimeTo).ToListAsync();
            return listTerm;
        }

        public async Task<IEnumerable<ProjectDto>> getTermProjectCompany(int termID)
        {
            var termProject = await _context.ProjectDetails
                                                        .Where(pj => pj.TermId == termID && pj.Project.ProjectStatus == Status.ACTIVE)
                                                        .Select(pj => new ProjectDto
                                                        {
                                                            ProjectId = pj.ProjectId,
                                                            ProjectName = pj.Project.ProjectName,
                                                        }).ToListAsync();
            return termProject;
        }

        public async Task<bool> UpdateDoneTerm(int termID)
        {
            TermSlouse termModel = await _context.TermSlice.FindAsync(termID);
            if (termModel == null) return false;
            termModel.TermStatus = Status.INACTIVE;

            _context.Entry(termModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
    public interface ITermProjectCompanyRepo
    {
        Task<IEnumerable<ProjectDto>> getTermProjectCompany(int termID);
        Task<IEnumerable<TermDto>> GetListTermCompany(String companyID);
        Task<bool> addTermCompany(AddTermDto term);
        Task<DateTime> GetLatestTimeto(String companyID);
        Task<bool> AddProjectToTerm(int termID, string projectID);
        Task<bool> UpdateDoneTerm(int termID);
    }
}
