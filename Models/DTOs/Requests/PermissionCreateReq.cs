using System.ComponentModel.DataAnnotations;
using TaskManagement.Models.Enum;

namespace TaskManagement.Models.DTOs.Requests
{
    public class PermissionCreateReq
    {
        public int TaskId { get; set; }
        public int AssigneeId { get; set; }

        [RegularExpression("^(Read|Write|Update|Delete)$", ErrorMessage = "Role must be Admin, User, or Manager.")]
        public PermissionType PermissionValue { get; set; }
    }
}
