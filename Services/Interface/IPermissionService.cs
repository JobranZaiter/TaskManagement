using TaskManagement.Models;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Models.DTOs.Responses;
using TaskManagement.Models.Enum;
using TaskManagement.Repository.Interface;

namespace TaskManagement.Services.Interface
{
    public interface IPermissionService
    {
        Task<(ErrorType HttpCode, string Message)> AddPermission(PermissionCreateReq request, int projectId);
        Task<(ErrorType HttpCode, string Message)> DeletePermission(int taskId, int permissionId, int projectId);
        Task<(ErrorType HttpCode, string Message, List<PermissionResp>? Permissions)> GetAllPermissionsForProject(int projectId);



    }
}
