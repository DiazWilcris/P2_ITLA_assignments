using System;
using System.Collections.Generic;
using System.Text;

namespace StarAtlas.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public ApiResponse(T data, string message = "Operation completed successfully.")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Data = default;
        }
    }
}
