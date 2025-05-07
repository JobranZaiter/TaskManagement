namespace TaskManagement.Models.DTOs.Requests
{
    public class SubTaskReq
    {
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Awaiting";
        public int AssigneeId { get; set; }
    }
}
