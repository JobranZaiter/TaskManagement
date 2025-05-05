using System.Security.Claims;
using TaskManagement.Models;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Models.DTOs.Responses;
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
        private readonly IPermissionManager permissionManager;

        public ProjectService(
            IHttpContextAccessor _contextAccessor,
            IProjectRepository _projectRepository,
            IUserRepository _userRepository,
            IPermissionManager _permissionManager)
        {
            contextAccessor = _contextAccessor;
            projectRepository = _projectRepository;
            userRepository = _userRepository;
            permissionManager = _permissionManager;
        }

        private int? GetCurrentUserId()
        {
            var id = contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out var userId) ? userId : null;
        }

        public async Task<(ErrorType HttpCode, string Message, List<OwnedProjResp>? Projects)> GetAllProjects()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first.", null);

            if (!await userRepository.UserExistsAsync(userId.Value))
                return (ErrorType.Unauthorized, "User does not exist", null);

            var projects = await projectRepository.GetAllByUserIdAsync(userId.Value);

            if (projects == null || projects.Count == 0)
                return (ErrorType.NotFound, "No projects found", null);
            List<OwnedProjResp> ownedProjects = projects.Select(p=> new OwnedProjResp
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description
            }).ToList(); ;

            return (ErrorType.Ok, "Projects found", ownedProjects);
        }

        public async Task<(ErrorType HttpCode, string Message, List<AssignedProjResp>? Projects)> GetProjectAssignments()
        {
            var id = GetCurrentUserId();
            if (id == null)
                return (ErrorType.Unauthorized, "Please login first.", null);
            if(!await userRepository.UserExistsAsync(id.Value))
                return (ErrorType.Unauthorized, "User does not exist", null);
            var projects = await projectRepository.GetProjectsByAssigneeId(id.Value);
            if (projects == null || projects.Count == 0)
                return (ErrorType.NotFound, "No projects found", null);
            List<AssignedProjResp> assignedProjects = (List<AssignedProjResp>)projects.Select(p => new AssignedProjResp
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Role = p.ProjectAssignees.FirstOrDefault(pa => pa.UserId == id.Value)?.Role ?? "No role assigned"
            }).ToList();
            return (ErrorType.Ok, "Assigned Projects :", assignedProjects);
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
        public async Task<(ErrorType HttpCode, string Message, ProjectDetailsResp? Project)> GetProjectDetails(int projectId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return (ErrorType.Unauthorized, "Please login first", null);

            if (!await userRepository.UserExistsAsync(userId.Value))
                return (ErrorType.Unauthorized, "User does not exist", null);

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found", null);

            var response = new ProjectDetailsResp
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Tasks = new List<TaskInfo>(),
                AssignedUsers = new List<ProjectAssigneeInfo>()
            };

            if (project.UserId != userId.Value)
            {
                foreach (var task in project.Tasks)
                {
                    if (await permissionManager.UserHasPermissionAsync(userId.Value, task.Id, PermissionType.Read))
                    {
                        response.Tasks.Add(new TaskInfo
                        {
                            Id = task.Id,
                            Title = task.Title,
                            Description = task.Description,
                            IsCompleted = task.isCompleted
                        });
                    }
                }
            }
            else
            {
                foreach (var task in project.Tasks)
                {
                    response.Tasks.Add(new TaskInfo
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        IsCompleted = task.isCompleted
                    });
                }
            }

            foreach (var assignee in project.ProjectAssignees)
            {
                response.AssignedUsers.Add(new ProjectAssigneeInfo
                {
                    Id = assignee.Id,
                    UserId = assignee.UserId,
                    Name = assignee.User.Name,
                    Role = assignee.Role
                });
            }

            return (ErrorType.Ok, "Project details found", response);
        }
        public async Task<(ErrorType HttpCode, string Message)> AddAssigneeByEmailAsync(int projectId, string assigneeEmail, string role)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
                return (ErrorType.Unauthorized, "Please login first");

            if (!await userRepository.UserExistsAsync(currentUserId.Value))
                return (ErrorType.Unauthorized, "Current user does not exist");

            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return (ErrorType.NotFound, "Project not found");

            if (project.UserId != currentUserId.Value)
                return (ErrorType.Forbidden, "Only the owner can add assignees");

            var userToAssign = await userRepository.GetByEmailAsync(assigneeEmail);
            if (userToAssign == null)
                return (ErrorType.NotFound, "Assignee user not found");

            if (project.ProjectAssignees.Any(pa => pa.UserId == userToAssign.Id))
                return (ErrorType.BadRequest, "User already assigned to this project");

            var newAssignee = new ProjectAssignee
            {
                ProjectId = projectId,
                UserId = userToAssign.Id,
                Role = role
            };

            project.ProjectAssignees.Add(newAssignee);
            await projectRepository.SaveChangesAsync();

            return (ErrorType.Ok, "Assignee added successfully");
        }



    }
}
