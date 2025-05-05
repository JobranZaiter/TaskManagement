namespace TaskManagement.Models.DTOs.Requests
{
    public class TaskCreateReq
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int AssigneeId { get; set; }
    }
}
