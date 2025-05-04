using TaskManagement.Models;
using TaskManagement.Models.Enum;

namespace TaskManagement.Services.Interface
{
    public interface ITaskService
    {
        Task<(ErrorType HttpCode, string Message, List<SubTask>? Tasks)> GetAllSubTasks();
        Task<(ErrorType HttpCode, string Message, List<AppTask>? Tasks)> GetAllProjectTasks(int projectId);
    }
}
