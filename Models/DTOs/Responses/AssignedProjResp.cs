namespace TaskManagement.Models.DTOs.Responses
{
    public class AssignedProjResp
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
