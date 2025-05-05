namespace TaskManagement.Models.DTOs.Responses
{
    public class AssignedSubTaskResp
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; } = null!;
        public string Status { get; set; }
    }
}
