﻿namespace TaskManagement.Models.Enum
{
    public enum ErrorType
    {
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}
