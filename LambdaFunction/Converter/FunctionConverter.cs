using LambdaFunction.Models;

namespace LambdaFunction.Converter
{
    public static class FunctionConverter
    {
        public static FullResponse ToFullResponse(bool success, Address address = null)
        {
            return new FullResponse()
            {
                Success = success,
                Address = address
            };
        }

        public static FullResponse ToExceptionResponse(this string exception, bool success)
        {
            return new FullResponse()
            {
                Success = success,
                Message = exception
            };
        }
    }
}
