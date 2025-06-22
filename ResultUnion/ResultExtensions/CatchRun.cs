namespace ResultUnion.ResultExtensions;

public static partial class ResultExtension
{
    public static Result<T, E> CatchRun<T, E>(Func<T> func)
        where E : Exception
    {
        try
        {
            return Result.Ok<T, E>(func());
        }
        catch (E e)
        {
            return Result.Err<T, E>(e);
        }
    }

    public static Result<T, Exception> CatchRun<T>(Func<T> func) => CatchRun<T, Exception>(func);

    public static E? CatchRun<E>(Action func)
        where E : Exception
    {
        try
        {
            func();
            return null;
        }
        catch (E e)
        {
            return e;
        }
    }

    public static Exception? CatchRun(Action func) => CatchRun<Exception>(func);
}
