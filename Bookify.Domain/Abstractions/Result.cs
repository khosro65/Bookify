namespace Bookify.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();

        if(!isSuccess && error != Error.None)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    }
public class Error
{
    public static Error None = new(string.Empty,string.Empty);
    public string Code { get; }
    public string Message { get; }
    protected internal Error(string code,string message)
    {
        Code = code;
        Message = message;
    }
}
