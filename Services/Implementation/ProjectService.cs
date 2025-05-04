using System.IdentityModel.Tokens.Jwt;
using TaskManagement.Models;
using TaskManagement.Models.DTOs;
using TaskManagement.Models.Enum;
using TaskManagement.Repository.Interface;
using TaskManagement.Services.Interface;

namespace TaskManagement.Services.Implementation
{
    public class ProjectService : IProjectService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IProjectRepository projectRepository;
        private readonly IUserRepository userRepository;

        public ProjectService(
            IHttpContextAccessor _contextAccessor,
            IProjectRepository _projectRepository,
            IUserRepository _userRepository)
        {
            contextAccessor = _contextAccessor;
            projectRepository = _projectRepository;
            userRepository = _userRepository;
        }

        private int? GetCurrentUserId()
        {
            var id = contextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return int.TryParse(id, out var userId) ? userId : null;
        }

        public async Task<(ErrorType HttpCode, string Message, List<Project>? Projects)> GetAllProjects()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Forbidden, "Please login first.", null);

            if (!await userRepository.UserExistsAsync(userId.Value))
                return (ErrorType.Forbidden, "User does not exist", null);

            var projects = await projectRepository.GetAllByUserIdAsync(userId.Value);

            if (projects == null || projects.Count == 0)
                return (ErrorType.NotFound, "No projects found", null);

            return (ErrorType.Ok, "Projects found", projects);
        }

        public async Task<(ErrorType HttpCode, string Message)> AddProject(ProjectReq req)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first");

            if (!await userRepository.UserExistsAsync(userId.Value))
                return (ErrorType.Unauthorized, "User does not exist");

            var user = await userRepository.GetByIdAsync(userId.Value);

            var project = new Project
            {
                UserId = user.Id,
                Title = req.Title,
                Description = req.Description
            };

            await projectRepository.AddAsync(project);
            await projectRepository.SaveChangesAsync();

            return (ErrorType.Ok, "Project added successfully");
        }

        public async Task<(ErrorType HttpCode, string Message)> DeleteProject(int projectId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first");

            if (!await userRepository.UserExistsAsync(userId.Value))
                return (ErrorType.Unauthorized, "User does not exist");

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found");

            if (project.UserId != userId.Value)
                return (ErrorType.Forbidden, "You are not the owner of this project");

            await projectRepository.DeleteAsync(project);
            await projectRepository.SaveChangesAsync();

            return (ErrorType.Ok, "Project deleted successfully");
        }

        public async Task<(ErrorType HttpCode, string Message, List<Project>? Projects)> GetProjectAssignments()
        {
            var id = GetCurrentUserId();
            if (id == null)
                return (ErrorType.Unauthorized, "Please login first.", null);
            if(!await userRepository.UserExistsAsync(id.Value))
                return (ErrorType.Unauthorized, "User does not exist", null);
            var projects = await projectRepository.GetProjectsByAssigneeId(id.Value);
            if (projects == null || projects.Count == 0)
                return (ErrorType.NotFound, "No projects found", null);
            return (ErrorType.Ok, "Assigned Projects :", projects);
        }
    }
}
