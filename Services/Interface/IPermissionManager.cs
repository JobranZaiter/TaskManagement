﻿using TaskManagement.Models.Enum;

namespace TaskManagement.Services.Interface
{
    public interface IPermissionManager
    {
        Task<bool> UserHasPermissionAsync(int userId, int id, PermissionType read);
    }
}
