namespace E_Club.Errors;

public static class ResultPipelineExtensions
{
    // Tap: Execute an action without changing the result
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);
        return result;
    }

    public static Result Tap(this Result result, Action action)
    {
        if (result.IsSuccess)
            action();
        return result;
    }

    // Map: Transform the value
    public static Result<TNew> Map<T, TNew>(this Result<T> result, Func<T, TNew> mapper)
    {
        return result.IsSuccess
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TNew>(result.Error);
    }

    // Bind: Chain operations that return Result
    public static Result<TNew> Bind<T, TNew>(this Result<T> result, Func<T, Result<TNew>> binder)
    {
        return result.IsSuccess
            ? binder(result.Value)
            : Result.Failure<TNew>(result.Error);
    }

    // Ensure: Add a condition check
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(error);
    }

    // Finally: Execute an action at the end
    public static void Finally<T>(this Result<T> result, Action<T?> onSuccess, Action<Error> onFailure)
    {
        if (result.IsSuccess)
            onSuccess(result.Value);
        else
            onFailure(result.Error);
    }
}