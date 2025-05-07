namespace TaskManagement.Models.DTOs.Responses
{
    public class PermissionResp
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string TaskTitle { get; set; }
        public string Permission { get; set; }
    }
}
