namespace AspNetCoreEcommerce.Shared
{
    public class ResultPattern
    {
        public bool Success { get; private set; }
        public object? Error { get; private set; }
        public object? Data { get; private set; }

        public static ResultPattern SuccessResult(object data)
        {
            return new ResultPattern
            {
                Success = true,
                Data = data
            };
        }

        public static ResultPattern FailResult(object error)
        {
            return new ResultPattern
            {
                Success = false,
                Error = error
            };
        }
    }
}