using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEcommerce.Result
{
    public class ResultPattern
    {
        private int StatusCode { get; set; }
        public bool Success { get; set; }
        private string? Message { get; set; }
        private object? Data { get; set; }

        public static ResultPattern SuccessResult(object data, string message)
        {
            return new ResultPattern
            {
                StatusCode = 200,
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ResultPattern FailResult(string message, int statusCode = 400)
        {
            return new ResultPattern
            {
                StatusCode = statusCode,
                Success = false,
                Message = message
            };
        }
    }
}