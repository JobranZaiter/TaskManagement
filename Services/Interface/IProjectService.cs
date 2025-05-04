using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Models.Enum;

namespace TaskManagement.Services.Interface
{
    public interface IProjectService
    {
        Task<(ErrorType HttpCode, string Message, List<Project>? Projects)> GetAllProjects();
        Task<(ErrorType HttpCode, string Message)> AddProject(ProjectReq req);
        Task<(ErrorType HttpCode, string Message)> DeleteProject(int projectId);
        Task<(ErrorType HttpCode, string Message, List<Project>? Projects)> GetProjectAssignments();
    }
}
