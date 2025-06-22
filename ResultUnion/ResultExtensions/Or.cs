namespace ResultUnion.ResultExtensions;

public static partial class ResultExtension
{
    /// <summary>
    /// alias for OrElse
    /// </summary>
    public static Result<T, U> Or<T, E, U>(this Result<T, E> result, Func<E, Result<T, U>> func) =>
        result.OrElse(func);
}
