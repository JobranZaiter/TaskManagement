using TaskManagement.Models;
using TaskManagement.Models.DTOs.Requests;
using TaskManagement.Models.DTOs.Responses;
using TaskManagement.Models.Enum;

namespace TaskManagement.Services.Interface
{
    public interface ITaskService
    {
        Task<(ErrorType HttpCode, string Message, List<AssignedSubTaskResp>? Tasks)> GetAllSubTasks();
        Task<(ErrorType HttpCode, string Message, List<AppTask>? Tasks)> GetAllProjectTasks(int projectId);
        Task<(ErrorType HttpCode, string Message, List<SubTaskResp>? SubTasks)>GetSubTasks(int taskId);
        Task<(ErrorType HttpError, string Message)> DeleteTask(int taskId);
        Task<(ErrorType HttpCode, string Message)> AddSubTaskToTaskAsync(int taskId, SubTaskReq req);
        Task<(ErrorType HttpCode, string Message)> DeleteSubTaskFromTaskAsync(int taskId, int subTaskId);
        Task<(ErrorType HttpCode, string Message)> UpdateSubTask(int taskId, int subTaskId, string status);
    }
}
