using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEcommerce.DTOs
{
    public class ApiResponse<T>
    {
        public int StatusCode {get; set;}
        public bool Success {get; set;}
        public T Data {get; set;}
        public List<string> Errors {get; set;}

        public ApiResponse(int statusCode, T data)
        {
            Success = true;
            Errors = [];
        }

        public ApiResponse(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            Success = false;
            Errors = errors;
        }

        public ApiResponse(int statusCode, string error)
        {
            StatusCode = statusCode;
            Success = false;
            Errors = [error];
        }
    }
}