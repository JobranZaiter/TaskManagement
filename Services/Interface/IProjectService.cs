using TaskManagement.Models;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Models.DTOs.Responses;
using TaskManagement.Models.Enum;

namespace TaskManagement.Services.Interface
{
    public interface IProjectService
    {
        Task<(ErrorType HttpCode, string Message, List<OwnedProjResp>? Projects)> GetAllProjects();
        Task<(ErrorType HttpCode, string Message)> AddProject(ProjectReq req);
        Task<(ErrorType HttpCode, string Message)> DeleteProject(int projectId);
        Task<(ErrorType HttpCode, string Message, List<AssignedProjResp>? Projects)> GetProjectAssignments();
        Task<(ErrorType HttpCode, string Message, ProjectDetailsResp? Project)> GetProjectDetails(int projectId);
        Task<(ErrorType HttpCode, string Message)> AddAssigneeByEmailAsync(int projectId, string assigneeEmail, string role);
    }
}
