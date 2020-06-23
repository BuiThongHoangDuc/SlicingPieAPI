using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly SWD_SlicingPieContext _context;
        public ProjectRepository(SWD_SlicingPieContext context)
        {
            _context = context;
        }

        public IQueryable<ProjectDto> getProjectList(string companyID)
        {
            var listProject = _context.Projects
                                        .Where(pj => pj.CompanyId == companyID && pj.ProjectStatus == Status.ACTIVE)
                                        .Select(pj => new ProjectDto
                                        {
                                            ProjectId = pj.ProjectId,
                                            ProjectName = pj.ProjectName,
                                        });
            return listProject;
        }
    }

    public interface IProjectRepository
    {
        IQueryable<ProjectDto> getProjectList(string companyID);
    }
}
