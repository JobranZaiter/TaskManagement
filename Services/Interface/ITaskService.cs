using TaskManagement.Models;
using TaskManagement.Models.DTOs.Responses;
using TaskManagement.Models.Enum;

namespace TaskManagement.Services.Interface
{
    public interface ITaskService
    {
        Task<(ErrorType HttpCode, string Message, List<AssignedSubTaskResp>? Tasks)> GetAllSubTasks();
        Task<(ErrorType HttpCode, string Message, List<AppTask>? Tasks)> GetAllProjectTasks(int projectId);
    }
}
