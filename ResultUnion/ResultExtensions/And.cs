namespace ResultUnion.ResultExtensions;

public static partial class ResultExtension
{
    /// <summary>
    /// alias for AndThen
    /// </summary>
    public static Result<U, E> And<T, E, U>(this Result<T, E> result, Func<T, Result<U, E>> func) =>
        result.AndThen(func);
}
