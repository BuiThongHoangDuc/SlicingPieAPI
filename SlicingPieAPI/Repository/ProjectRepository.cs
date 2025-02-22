﻿using Microsoft.EntityFrameworkCore;
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
        private readonly SWDSlicingPieContext _context;
        public ProjectRepository(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public async Task AddProject(ProjectDto project)
        {
            Project projectModel = new Project();
            projectModel.ProjectId = project.ProjectId;
            projectModel.ProjectName = project.ProjectName;
            projectModel.ProjectStatus = Status.ACTIVE;
            projectModel.CompanyId = project.CompanyId;

            _context.Projects.Add(projectModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public bool deleteProject(string projectid)
        {
            var project = _context.Projects.Find(projectid);
            if (project == null) return false;
            else
            {
                project.ProjectStatus = Status.INACTIVE;
                _context.SaveChanges();
                return true;
            }
        }

        public async Task<string> getProjectLast(string companyID)
        {
            var lastID = await _context.Projects.Where(pj => pj.ProjectStatus == Status.ACTIVE && pj.CompanyId == companyID).OrderByDescending(pj => pj.ProjectId).Select(project => project.ProjectId).FirstOrDefaultAsync();
            return lastID;
        }

        public IQueryable<ProjectDto> getProjectList(string companyID)
        {
            var listProject = _context.Projects
                                        .Where(pj => pj.CompanyId == companyID && pj.ProjectStatus == Status.ACTIVE)
                                        .Select(pj => new ProjectDto
                                        {
                                            ProjectId = pj.ProjectId,
                                            ProjectName = pj.ProjectName,
                                            CompanyId = pj.CompanyId,
                                            ProjectStatus = pj.ProjectStatus,
                                        }) ;
            return listProject;
        }

        public async Task<string> udpateProject(ProjectDto project, String projectid)
        {
            Project dto = await _context.Projects.FindAsync(projectid);
            dto.ProjectName = project.ProjectName;

            _context.Entry(dto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return dto.CompanyId;
        }
    }

    public interface IProjectRepository
    {
        IQueryable<ProjectDto> getProjectList(string companyID);
        Task AddProject(ProjectDto project);
        Task<String> getProjectLast(String companyID);
        Task<String> udpateProject(ProjectDto project, String projectid);
        bool deleteProject(string projectid);

    }
}
