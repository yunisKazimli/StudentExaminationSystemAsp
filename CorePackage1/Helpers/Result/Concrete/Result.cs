using CorePackage.Helpers.Result.Abstract;

namespace CorePackage.Helpers.Result.Concrete
{
    public class Result : IResult
    {
        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }
    }
}