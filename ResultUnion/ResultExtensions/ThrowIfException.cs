namespace ResultUnion.ResultExtensions;

public static partial class ResultExtension
{
    public static T ThrowIfException<T, E>(this Result<T, E> result)
        where E : Exception
    {
        if (result.IsErr())
            throw result.UnwrapErr();
        return result.Unwrap();
    }
}
