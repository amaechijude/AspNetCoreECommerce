namespace AspNetCoreEcommerce.ResultResponse
{
    public class ResultPattern
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

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

        public static ResultPattern BadModelState(object? data)
        {
            return new ResultPattern
            {
                StatusCode = 400,
                Success = false,
                Message = "Invalid model state",
                Data = data
            };
        }
    }
}