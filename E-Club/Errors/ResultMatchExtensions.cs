namespace E_Club.Errors;

public static class ResultMatchExtensions
{
    public static TResult Match<T, TResult>(
        this Result<T> result,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);
    }

    public static TResult Match<TResult>(
        this Result result,
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess()
            : onFailure(result.Error);
    }

    // Async versions
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Match(onSuccess, onFailure);
    }

    public static async Task<TResult> MatchAsync<TResult>(
        this Task<Result> resultTask,
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Match(onSuccess, onFailure);
    }
}