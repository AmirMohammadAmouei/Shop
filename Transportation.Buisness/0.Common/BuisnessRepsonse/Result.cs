using System.Globalization;

namespace Transportation.Buisness._0.Common
{
    public class Result
    {
        public bool IsSucceeded { get; protected set; }
        public string Message { get; protected set; }
        public IEnumerable<string> Errors { get; protected set; } = Enumerable.Empty<string>();
        public bool HasErrors => Errors.Any();

        protected Result() { }

        public static Result Success(string message = "")
            => new() { IsSucceeded = true, Message = message };

        public static Result Failed(string message = "")
            => new() { IsSucceeded = false, Message = message };

        public static Result Failed(IEnumerable<string> errors)
            => new() { IsSucceeded = false, Errors = errors };

        public static Result Failed(string message, IEnumerable<string> errors)
            => new() { IsSucceeded = false, Message = message, Errors = errors };
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; }

        private Result() { }

        public static Result<T> Success(T data, string message = "")
            => new() { IsSucceeded = true, Data = data, Message = message };

        public static new Result<T> Failed(string message = "")
            => new() { IsSucceeded = false, Data = default, Message = message };

        public static new Result<T> Failed(IEnumerable<string> errors)
            => new() { IsSucceeded = false, Data = default, Errors = errors };

        public static new Result<T> Failed(string message, IEnumerable<string> errors)
            => new() { IsSucceeded = false, Data = default, Message = message, Errors = errors };
    }
  
}
