namespace TaskManagement.Models.DTOs.Responses
{
    public class SubTaskResp
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int AssigneeId { get; set; }

    }
}
